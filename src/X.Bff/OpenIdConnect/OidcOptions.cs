// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;

namespace X.Bff.OpenIdConnect;

/// <summary>
/// Options for Oidc
/// </summary>
public class OidcOptions
{
    public string Authority { get; set; } = string.Empty;

    public bool RequireHttps { get; set; } = true;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string[] Scopes { get; set; } = Array.Empty<string>();
}
