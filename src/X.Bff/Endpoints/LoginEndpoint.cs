// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.Bff.Extensions;

namespace X.Bff.Endpoints;

internal class LoginEndpoint
{
    public static async Task HandleRequest(HttpContext context, [FromServices] ILogger<LoginEndpoint> logger)
    {
        logger.LogDebug("Start handle login request.");

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

        logger.LogDebug("Challenge with returnUrl {returnUrl}.", returnUrl);

        await context.ChallengeAsync(new AuthenticationProperties { RedirectUri = returnUrl });
    }
}
