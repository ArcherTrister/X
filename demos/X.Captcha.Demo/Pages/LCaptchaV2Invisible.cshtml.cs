// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XhCaptcha
// for more information concerning the license and the contributors participating to this project.

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.RazorPages;

using X.Captcha;
using X.Captcha.L;

namespace X.Captcha.Demo.Pages;

public class LCaptchaV2InvisibleModel : PageModel
{
    private readonly ILCaptchaV2SiteVerify _siteVerify;

    public string Result { get; set; }

    public LCaptchaV2InvisibleModel(ILCaptchaV2SiteVerify siteVerify)
    {
        _siteVerify = siteVerify;
    }

    public async Task OnPostAsync(string token)
    {
        var response = await _siteVerify.Verify(new CaptchaSiteVerifyRequest
        {
            Response = token,
            RemoteIp = HttpContext.Connection.RemoteIpAddress?.ToString()
        });

        this.Result = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
