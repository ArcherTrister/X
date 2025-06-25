// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

namespace X.Swashbuckle.Middlewares;

public class SwaggerBasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _userName;
    private readonly string _password;

    public SwaggerBasicAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _userName = configuration["SwaggerAuth:UserName"];
        _password = configuration["SwaggerAuth:Password"];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase))
        {
            // 检查 Authorization 头
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic ", StringComparison.Ordinal))
            {
                // 解析凭据
                var header = AuthenticationHeaderValue.Parse(authHeader);
                var credentialBytes = Convert.FromBase64String(header.Parameter!);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                // 验证凭据
                if (await IsAuthorized(credentials[0], credentials[1]))
                {
                    await _next(context);
                    return;
                }
            }

            // 返回 401 要求认证
            context.Response.Headers[HeaderNames.WWWAuthenticate] = "Basic";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }

    protected virtual Task<bool> IsAuthorized(string username, string password)
    {
        var result = username.Equals(_userName, StringComparison.OrdinalIgnoreCase)
               && password.Equals(_password, StringComparison.Ordinal);
        return Task.FromResult(result);
    }
}
