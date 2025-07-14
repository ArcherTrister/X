// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
#if NET8_0_OR_GREATER
using Duende.IdentityModel;
#else
using IdentityModel;
#endif
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using X.Bff.OpenIdConnect;

namespace X.Bff.Endpoints;

internal class BackChannelLogoutEndpoint
{
    public static async Task<IResult> HandleRequest(
        HttpContext context,
        [FromServices] IOptionsSnapshot<OpenIdConnectOptions> oidcOptions,
        [FromServices] LogoutSessionManager logoutSessionManager,
        [FromServices] ILogger<BackChannelLogoutEndpoint> logger)
    {
        logger.LogDebug("Start handle back channel logout request.");

        // 1. 设置安全响应头
        context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
        context.Response.Headers.Append("Pragma", "no-cache");

        try
        {
            // 2. 严格检查内容类型
            if (context.Request.HasFormContentType)
            {
                // 3. 从表单数据中获取logout_token
                var form = await context.Request.ReadFormAsync();
                var logoutToken = form[OidcConstants.BackChannelLogoutRequest.LogoutToken].FirstOrDefault();

                if (string.IsNullOrEmpty(logoutToken))
                {
                    logger.LogWarning("Back-channel logout request missing logout_token.");
                }
                else
                {
                    // 4. 验证登出令牌
                    var user = await ValidateLogoutTokenAsync(logoutToken, oidcOptions.Value);

                    // 5. 提取关键声明
                    var sub = user.FindFirstValue(JwtClaimTypes.Subject);
                    var sid = user.FindFirstValue(JwtClaimTypes.SessionId);

                    // 6. 存储会话标识以便后续登出操作
                    logoutSessionManager.Add(sub, sid);
                    logger.LogInformation("Processed back-channel logout for subject {sub}, session {sid}.", sub, sid);

                    // 7. 安全完成响应
                    return Results.Ok();
                }
            }
            else
            {
                logger.LogWarning(
                    "Invalid content-type for back-channel logout: {ContentType}. Expecting application/x-www-form-urlencoded.",
                    context.Request.ContentType);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing back-channel logout.");
        }

        return Results.BadRequest();
    }

    private static async Task<ClaimsPrincipal> ValidateLogoutTokenAsync(string logoutToken, OpenIdConnectOptions oidcOptions)
    {
        var claims = await ValidateJwt(logoutToken, oidcOptions);

        if (claims.FindFirst("sub") == null && claims.FindFirst("sid") == null)
        {
            throw new Exception("Invalid logout token");
        }

        var nonce = claims.FindFirstValue("nonce");
        if (!string.IsNullOrWhiteSpace(nonce))
        {
            throw new Exception("Invalid logout token");
        }

        var eventsJson = claims.FindFirst("events")?.Value;
        if (string.IsNullOrWhiteSpace(eventsJson))
        {
            throw new Exception("Invalid logout token");
        }

        // var events = JObject.Parse(eventsJson);
        // var logoutEvent = events.TryGetValue("http://schemas.openid.net/event/backchannel-logout");
        // if (logoutEvent == null) throw new Exception("Invalid logout token");
        var events = JsonDocument.Parse(eventsJson);
        if (!events.RootElement.TryGetProperty("http://schemas.openid.net/event/backchannel-logout", out _))
        {
            throw new Exception("Invalid logout token");
        }

        return claims;
    }

    private static async Task<ClaimsPrincipal> ValidateJwt(string jwt, OpenIdConnectOptions oidcOptions)
    {
        // read discovery document to find issuer and key material
        // var client = new HttpClient();
        // var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);
        //
        // var keys = new List<SecurityKey>();
        // foreach (var webKey in disco.KeySet.Keys)
        // {
        //     var e = Base64Url.Decode(webKey.E);
        //     var n = Base64Url.Decode(webKey.N);
        //
        //     var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
        //     {
        //         KeyId = webKey.Kid
        //     };
        //
        //     keys.Add(key);
        // }
        var oidcConfiguration =
            await oidcOptions.ConfigurationManager?.GetConfigurationAsync(CancellationToken.None)!;

        var parameters = new TokenValidationParameters
        {
            ValidIssuer = oidcConfiguration.Issuer,
            ValidAudience = oidcOptions.ClientId,
            IssuerSigningKeys = oidcConfiguration.SigningKeys,
            NameClaimType = JwtClaimTypes.Name,
            RoleClaimType = JwtClaimTypes.Role,
            RequireSignedTokens = true,
        };

        var handler = new JwtSecurityTokenHandler();
        handler.InboundClaimTypeMap.Clear();

        return handler.ValidateToken(jwt, parameters, out _);
    }
}
