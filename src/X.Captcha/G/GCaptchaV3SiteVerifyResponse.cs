// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System.Text.Json.Serialization;

namespace X.Captcha.G;
public class GCaptchaV3SiteVerifyResponse : GCaptchaV2SiteVerifyResponse
{
    // the score for this request (0.0 - 1.0)
    [JsonPropertyName("score")]
    public float Score { get; set; }

    // the action name for this request (important to verify)
    [JsonPropertyName("action")]
    public string Action { get; set; }
}
