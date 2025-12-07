// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System.Threading.Tasks;

namespace X.Captcha.Re;

public interface IReCaptchaV2SiteVerify
{
    Task<ReCaptchaV2SiteVerifyResponse> Verify(CaptchaSiteVerifyRequest request);
}
