// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

namespace X.Captcha;

public class CaptchaOptions
{
    /// <summary>
    /// https://api.hcaptcha.com/siteverify.
    /// https://captcha.luosimao.com/api/site_verify.
    /// https://www.google.com/recaptcha/api/siteverify.
    /// </summary>
    public string VerifyBaseUrl { get; set; }

    public string SiteKey { get; set; }

    public string SiteSecret { get; set; }
}
