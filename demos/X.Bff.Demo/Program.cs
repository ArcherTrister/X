using X.Bff;
using X.Bff.OpenIdConnect;
using X.Bff.Yarp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

// builder.Services.AddHttpContextAccessor();
var bffOptions = builder.Configuration
    .GetSection("Bff")
    .Get<BffOptions>() ?? new BffOptions();
var oidcOptions = builder.Configuration
    .GetSection("Bff:Oidc")
    .Get<OidcOptions>() ?? new OidcOptions();
var reverseProxyOptions = builder.Configuration
    .GetSection("Bff:ReverseProxy")
    .Get<ReverseProxyOptions>() ?? new ReverseProxyOptions();

builder.Services.AddBff(reverseProxyOptions, o =>
{
    o.EndpointPrefix = bffOptions.EndpointPrefix;
    o.CacheExpirationInDays = bffOptions.CacheExpirationInDays;
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = bffOptions.CookieName;
        options.Cookie.SameSite = bffOptions.CookieSameSite;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = oidcOptions.Authority;
        options.ClientId = oidcOptions.ClientId;
        options.ClientSecret = oidcOptions.ClientSecret;
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
        options.MapInboundClaims = false;

        options.Scope.Clear();
        foreach (var scope in oidcOptions.Scopes)
        {
            options.Scope.Add(scope);
        }

        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBffEndpoints();

app.MapControllers();

/************************************************************************************************
 * The code below is the fallback to the SPA. It will serve the index.html file for all requests that
 * are not handled by the API endpoints. This is typical in a SPA application where the frontend handles
 * routing and the backend serves the initial HTML file.
 ************************************************************************************************/
app.MapFallbackToFile("/index.html")
    .AllowAnonymous();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.MapGet("/todos", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new Todo
                (
                    index,
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetTodos").RequireAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record Todo(int Id, DateOnly Date, string? Name);
