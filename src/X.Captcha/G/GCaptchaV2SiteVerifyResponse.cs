// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Text.Json.Serialization;

namespace X.Captcha.G;

public class GCaptchaV2SiteVerifyResponse : CaptchaSiteVerifyResponse
{
    // whether this request was a valid gCaptcha token for your site
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTs { get; set; }

    // the hostname of the site where the gCaptcha was solved
    [JsonPropertyName("hostname")]
    public string HostName { get; set; }

    [JsonPropertyName("error-codes")]
    public string[] ErrorCodes { get; set; }
}
