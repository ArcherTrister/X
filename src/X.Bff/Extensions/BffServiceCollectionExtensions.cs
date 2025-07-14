// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using X.Bff;
using X.Bff.OpenIdConnect;
using X.Bff.Yarp;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up bff services in an <see cref="IServiceCollection" />.
/// </summary>
public static class BffServiceCollectionExtensions
{
    public static BffBuilder AddBff(this IServiceCollection services, ReverseProxyOptions config, Action<BffOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.Configure(configureOptions);

        services.AddDistributedMemoryCache();

        services.AddSingleton<LogoutSessionManager>();

        // Set default cookie options for OpenIdConnect. This is used to handle token expiration for downstream API calls.
        services.AddTransient<OpenIdConnectCookieEventHandler>();
        services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, OpenIdConnectCookieAuthenticationOptions>();
        services.AddSingleton<IPostConfigureOptions<OpenIdConnectOptions>, DefaultOpenIdConnectOptions>();

        /**************************************************************************************
         * Registers support for managing access tokens used when calling downstream APIs
         * on behalf of the authenticated user. This setup handles among other things storage, automatic renewal,
         * and revocation of OpenID Connect tokens. Options set below is samples and is most likely not needed. It depends on the use cases.
         **************************************************************************************/
        services.AddOpenIdConnectAccessTokenManagement();

        services.AddHttpForwarder();

        services.AddReverseProxy().LoadFromMemory(config.Routes.ToRouteConfig(), config.Clusters.ToClusterConfig())
            .AddTransforms<HttpHeaderTransformation>();

        return new BffBuilder(services);
    }
}
