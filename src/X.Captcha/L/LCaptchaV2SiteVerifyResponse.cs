// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System.Text.Json.Serialization;

namespace X.Captcha.L;

public class LCaptchaV2SiteVerifyResponse : CaptchaSiteVerifyResponse
{
    /// <summary>
    /// 错误码
    /// -10 API KEY 为空
    /// -11 response为空
    /// -2x response错误
    /// -40 API_KEY使用错误，请确认使用了正确的KEY，注意前端和后端使用的KEY不同
    /// </summary>
    [JsonPropertyName("error")]
    public int Error { get; set; }

    /// <summary>
    /// 验证结果 success 验证成功 failed 验证失败
    /// </summary>
    [JsonPropertyName("res")]
    public string Res { get; set; }

    /// <summary>
    /// 错误代码描述
    /// </summary>
    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    // whether this request was a valid lCaptcha token for your site
    [JsonIgnore]
    public bool Success { get; set; } = true;

    [JsonIgnore]
    public string[] ErrorCodes { get; set; }
}
