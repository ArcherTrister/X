// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
#if NET8_0_OR_GREATER
using Duende.IdentityModel;
#else
using IdentityModel;
#endif
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.Bff.Extensions;

namespace X.Bff.Endpoints;

internal class LogoutEndpoint
{
    public static async Task HandleRequest(
        HttpContext context,
        [FromServices] IAuthenticationSchemeProvider authenticationSchemeProvider,
        [FromServices] ILogger<LogoutEndpoint> logger)
    {
        logger.LogDebug("Start handle logout request.");

        var returnUrl = context.Request.Query["returnUrl"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            if (!returnUrl.IsLocalUrl())
            {
                throw new Exception($"returnUrl is not valid: {returnUrl}");
            }
        }
        else
        {
            returnUrl = "/";
        }

        var sid = context.Request.Query[JwtClaimTypes.SessionId].FirstOrDefault();

        if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
        {
            var userSid = context.User.FindFirstValue(JwtClaimTypes.SessionId);
            if (!string.IsNullOrWhiteSpace(userSid))
            {
                if (userSid != sid)
                {
                    throw new Exception("Session id is invalid");
                }
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(sid))
            {
                logger.LogDebug("The user has logged out, jump directly.");

                context.Response.Redirect(returnUrl);
                return;
            }
        }

        // var defaultScheme = await authenticationSchemeProvider.GetDefaultSignOutSchemeAsync();
        // var cookieScheme = await context.GetCookieAuthenticationSchemeAsync();
        //
        // if ((scheme == null && defaultScheme?.Name == cookieScheme) || scheme == cookieScheme)
        // {
        //     // this sets a flag used by middleware to do post-signout work.
        //     context.SetSignOutCalled();
        // }
        //
        // await _inner.SignOutAsync(context, scheme, properties);
        var defaultScheme = await authenticationSchemeProvider.GetDefaultSignInSchemeAsync();
        await context.SignOutAsync(defaultScheme?.Name);

        logger.LogDebug("SignOut with returnUrl {returnUrl}.", returnUrl);

        await context.SignOutAsync(new AuthenticationProperties { RedirectUri = returnUrl });
    }
}
