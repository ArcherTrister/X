// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.AccessTokenManagement.OpenIdConnect;
#if NET8_0_OR_GREATER
using Duende.IdentityModel;
#else
using IdentityModel;
#endif
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace X.Bff.OpenIdConnect;

/// <summary>
/// Default implementation of <see cref="CookieAuthenticationEvents"/> for web clients using OpenId Connect with downstream API calls
/// with refresh token. original source: https://github.com/IdentityServer/IdentityServer4.Samples/tree/release/Clients/src/MvcHybridBackChannel
/// </summary>
public class OpenIdConnectCookieEventHandler(ILogger<OpenIdConnectCookieEventHandler> logger)
    : CookieAuthenticationEvents
{
    private const string ExpiresAt = "expires_at";

    /// <summary>
    /// Validate the principal and check if the access token or refresh token is expired.
    /// This is not needed if you do not have an downstream API that requires access tokens.
    /// </summary>
    /// <param name="context">CookieValidatePrincipalContext</param>
    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        if (context.Principal?.Identity is not null && context.Principal.Identity.IsAuthenticated)
        {
            var sub = context.Principal.FindFirstValue(JwtClaimTypes.Subject);
            var sid = context.Principal.FindFirstValue(JwtClaimTypes.SessionId);

            var logoutSessionManager =
                context.HttpContext.RequestServices.GetRequiredService<LogoutSessionManager>();
            if (await logoutSessionManager.IsLoggedOutAsync(sub, sid))
            {
                logger.LogWarning("User {Subject} session {SessionId} has been logged out", sub, sid);
                context.RejectPrincipal();
                return;
            }

            var tokens = context.Properties.GetTokens();
            var accessToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.AccessToken);
            var refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);

            if (accessToken == null || string.IsNullOrEmpty(accessToken.Value) || refreshToken == null ||
                string.IsNullOrEmpty(refreshToken.Value))
            {
                logger.LogError("Access token or refresh token is missing. Rejecting principal and renewing cookie.");
                context.RejectPrincipal();
                return;
            }

            if (!DateTimeOffset.TryParse(context.Properties.GetTokenValue(ExpiresAt), out var expiresAt))
            {
                return;
            }

            if (expiresAt <= DateTimeOffset.UtcNow)
            {
                var userTokenEndpointService =
                    context.HttpContext.RequestServices.GetRequiredService<IUserTokenEndpointService>();
#if NET7_0_OR_GREATER
                var userToken = await userTokenEndpointService.RefreshAccessTokenAsync(
                    new UserToken { RefreshToken = refreshToken.Value }, new UserTokenRequestParameters());
#else
                var userToken = await userTokenEndpointService.RefreshAccessTokenAsync(refreshToken.Value, new UserTokenRequestParameters());
#endif
                if (userToken.IsError)
                {
                    logger.LogError("Refresh token is expired. Rejecting principal so that the user can re-authenticate");
                    context.RejectPrincipal();
                    return;
                }
            }
        }
    }

    /// <summary>
    /// This method is called when the user is signing out. Revokes refresh tokens.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task SigningOut(CookieSigningOutContext context)
    {
        await context.HttpContext.RevokeRefreshTokenAsync();

        await base.SigningOut(context);
    }
}
