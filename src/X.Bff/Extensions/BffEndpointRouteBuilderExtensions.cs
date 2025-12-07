// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using X.Bff;
using X.Bff.Endpoints;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class BffEndpointRouteBuilderExtensions
{
    public static void MapBffEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var options = endpoints.ServiceProvider.GetRequiredService<IOptions<BffOptions>>().Value;

        endpoints.MapGet($"{options.EndpointPrefix}/login", LoginEndpoint.HandleRequest).AllowAnonymous();
        endpoints.MapGet($"{options.EndpointPrefix}/me", MeEndpoint.HandleRequest).AllowAnonymous();
        endpoints.MapGet($"{options.EndpointPrefix}/logout", LogoutEndpoint.HandleRequest).AllowAnonymous();
        endpoints.MapPost($"{options.EndpointPrefix}/back-channel-logout", BackChannelLogoutEndpoint.HandleRequest).AllowAnonymous();

        endpoints.MapReverseProxy();
    }
}
