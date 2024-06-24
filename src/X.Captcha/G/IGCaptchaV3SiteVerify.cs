// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System.Threading.Tasks;

namespace X.Captcha.G;

public interface IGCaptchaV3SiteVerify
{
    Task<GCaptchaV3SiteVerifyResponse> Verify(CaptchaSiteVerifyRequest request);
}
