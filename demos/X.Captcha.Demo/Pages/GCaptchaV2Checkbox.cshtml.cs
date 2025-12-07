using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.RazorPages;

using X.Captcha;
using X.Captcha.G;

namespace X.Captcha.Demo.Pages;

public class GCaptchaV2Checkbox : PageModel
{
    private readonly IGCaptchaV2SiteVerify _siteVerify;

    public string Result { get; set; }

    public GCaptchaV2Checkbox(IGCaptchaV2SiteVerify siteVerify)
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

        Result = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
