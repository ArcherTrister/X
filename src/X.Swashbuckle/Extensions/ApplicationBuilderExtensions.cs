// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using X.Swashbuckle.Middlewares;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerBasicAuth(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SwaggerBasicAuthMiddleware>();
    }
}
