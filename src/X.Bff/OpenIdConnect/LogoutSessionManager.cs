// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace X.Bff.OpenIdConnect;

// original source: https://github.com/IdentityServer/IdentityServer4.Samples/tree/release/Clients/src/MvcHybridBackChannel
public class LogoutSessionManager
{
    private static readonly object Lock = new();

    protected IDistributedCache Cache { get; }

    protected BffOptions Options { get; }

    protected ILogger<LogoutSessionManager> Logger { get; }

    public LogoutSessionManager(IDistributedCache cache, IOptions<BffOptions> options, ILogger<LogoutSessionManager> logger)
    {
        Cache = cache;
        Options = options.Value;
        Logger = logger;
    }

    public void Add(string sub, string sid)
    {
        Logger.LogWarning("BC Add a logout to the session: sub: {sub}, sid: {sid}", sub, sid);
        var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(Options.CacheExpirationInDays));

        lock (Lock)
        {
            var key = sub + sid;
            var logoutSession = Cache.GetString(key);
            Logger.LogInformation("BC logoutSession: {logoutSession}", logoutSession);
            if (logoutSession == null)
            {
                var newSession = new BackchannelLogoutSession { Sub = sub, Sid = sid };
                Cache.SetString(key, JsonSerializer.Serialize(newSession), options);
            }
        }
    }

    public async Task<bool> IsLoggedOutAsync(string sub, string sid)
    {
        Logger.LogInformation("BC IsLoggedOutAsync: sub: {sub}, sid: {sid}", sub, sid);
        var key = sub + sid;
        var matches = false;
        var logoutSession = await Cache.GetStringAsync(key);
        if (logoutSession != null)
        {
            var session = JsonSerializer.Deserialize<BackchannelLogoutSession>(logoutSession);
            if (session != null)
            {
                matches = session.IsMatch(sub, sid);
            }

            Logger.LogInformation("BC Logout session exists T/F {matches} : {sub}, sid: {sid}", matches, sub, sid);
        }

        return matches;
    }

    private record BackchannelLogoutSession
    {
        public string Sub { get; set; }

        public string Sid { get; set; }

        public bool IsMatch(string sub, string sid)
        {
            return (Sid == sid && Sub == sub) ||
                   (Sid == sid && Sub == null) ||
                   (Sid == null && Sub == sub);
        }
    }
}
