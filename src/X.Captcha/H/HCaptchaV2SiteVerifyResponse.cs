// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Text.Json.Serialization;

namespace X.Captcha.H;

public class HCaptchaV2SiteVerifyResponse : CaptchaSiteVerifyResponse
{
    /*
     {
       "success": true|false,     // is the passcode valid, and does it meet security criteria you specified, e.g. sitekey?
       "challenge_ts": timestamp, // timestamp of the challenge (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
       "hostname": string,        // the hostname of the site where the challenge was solved
       "credit": true|false,      // optional: deprecated field
       "error-codes": [...]       // optional: any error codes
       "score": float,            // ENTERPRISE feature: a score denoting malicious activity.
       "score_reason": [...]      // ENTERPRISE feature: reason(s) for score.
     }
     */

    // whether this request was a valid hCaptcha token for your site
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTs { get; set; }

    // the hostname of the site where the hCaptcha was solved
    [JsonPropertyName("hostname")]
    public string HostName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether optional: deprecated field.
    /// </summary>
    [JsonPropertyName("credit")]
    public bool Credit { get; set; }

    [JsonPropertyName("error-codes")]
    public string[] ErrorCodes { get; set; }

    // the score for this request (0.0 - 1.0)
    [JsonPropertyName("score")]
    public float Score { get; set; }

    // reason(s) for score.
    [JsonPropertyName("score_reason")]
    public string[] ScoreReason { get; set; }
}
