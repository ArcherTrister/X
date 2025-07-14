// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

#if NET8_0_OR_GREATER
using Duende.IdentityModel;
#else
using IdentityModel;
#endif
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace X.Bff.OpenIdConnect;

/// <summary>
/// Set default options for OpenIdConnect authentication.
/// </summary>
public class DefaultOpenIdConnectOptions : IPostConfigureOptions<OpenIdConnectOptions>
{
    /// <inheritdoc/>
    public void PostConfigure(string name, OpenIdConnectOptions options)
    {
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;

        // ReSharper disable once ConstantNullCoalescingCondition
        options.TokenValidationParameters ??= new TokenValidationParameters();
        if (string.IsNullOrEmpty(options.TokenValidationParameters.NameClaimType))
        {
            options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Subject;
        }
    }
}
