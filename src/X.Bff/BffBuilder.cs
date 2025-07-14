// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace X.Bff;

public sealed class BffBuilder(IServiceCollection services)
{
    /// <summary>
    /// Gets the services being configured.
    /// </summary>
    public IServiceCollection Services { get; } = services;

    private AuthenticationBuilder DefaultAuthentication { get; set; }

    public BffBuilder AddDefaultCookie(Action<CookieAuthenticationOptions> setupAction)
    {
        GetDefaultAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, setupAction);
        return this;
    }

    public BffBuilder AddDefaultOpenIdConnect(Action<OpenIdConnectOptions> setupAction)
    {
        GetDefaultAuthentication()
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, setupAction);
        return this;
    }

    public BffBuilder AddRedisCache(Action<RedisCacheOptions> setupAction)
    {
        Services.AddStackExchangeRedisCache(setupAction);
        return this;
    }

    private AuthenticationBuilder GetDefaultAuthentication() =>
        DefaultAuthentication ??= CreateDefaultAuthentication();

    private AuthenticationBuilder CreateDefaultAuthentication() =>
        Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
        });
}
