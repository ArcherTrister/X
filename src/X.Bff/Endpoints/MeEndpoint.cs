// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace X.Bff.Endpoints;

internal class MeEndpoint
{
    public static async Task<IResult> HandleRequest(HttpContext context, [FromServices] IClaimsTransformation claimsTransformation, [FromServices] ILogger<MeEndpoint> logger)
    {
        logger.LogDebug("Start handle user claims request.");

        context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";

        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            return Results.Unauthorized();
        }

        var result = await claimsTransformation.TransformAsync(context.User);
        var claims = result.Claims.Select(c => new
        {
            c.Type,
            c.Value,
        });

        return Results.Ok(claims);
    }
}
