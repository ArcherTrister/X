// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Identity;
using X.Swashbuckle.Middlewares;

namespace X.Swashbuckle.Demo;

public class CustomerSwaggerBasicAuthMiddleware : SwaggerBasicAuthMiddleware
{
    protected UserManager<IdentityUser> UserManager { get; }

    public CustomerSwaggerBasicAuthMiddleware(
        UserManager<IdentityUser> userManager,
        RequestDelegate next,
        IConfiguration configuration)
        : base(next, configuration)
    {
        UserManager = userManager;
    }

    protected override async Task<bool> IsAuthorized(string username, string password)
    {
        var user = await UserManager.FindByNameAsync(username);
        if (user == null)
        {
            return false;
        }

        return await UserManager.CheckPasswordAsync(user, password);
    }
}
