// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Http;

namespace X.Bff;

/// <summary>
/// Options for X.Bff
/// </summary>
public class BffOptions
{
    public string EndpointPrefix { get; set; } = "/bff";

    public string CookieName { get; set; } = "bff.cookie";

    public int CookieExpireTime { get; set; } = 28800;

    public bool CookieSlidingExpiration { get; set; }

    public SameSiteMode CookieSameSite { get; set; } = SameSiteMode.Lax;

    // Amount of time to check for old sessions. If this is to long, the cache will increase,
    // or if you have many user sessions, this will increase to much.
    public int CacheExpirationInDays { get; set; } = 8;
}
