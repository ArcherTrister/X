// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace X.Bff.OpenIdConnect;

/// <summary>
/// Setting default values for OpenIdConnect authentication.
/// </summary>
public class OpenIdConnectCookieAuthenticationOptions : IPostConfigureOptions<CookieAuthenticationOptions>
{
    protected BffOptions BffOptions { get; }

    public OpenIdConnectCookieAuthenticationOptions(IOptions<BffOptions> options)
    {
        BffOptions = options.Value;
    }

    /// <inheritdoc/>
    public void PostConfigure(string name, CookieAuthenticationOptions options)
    {
        if (name == CookieAuthenticationDefaults.AuthenticationScheme)
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromSeconds(BffOptions.CookieExpireTime);
            options.SlidingExpiration = true;
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.EventsType = typeof(OpenIdConnectCookieEventHandler);
        }
    }
}
