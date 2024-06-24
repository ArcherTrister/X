using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.RazorPages;

using X.Captcha;
using X.Captcha.Re;

namespace X.Captcha.Demo.Pages;

public class ReCaptchaV3Programmatically : PageModel
{
    private readonly IReCaptchaV3SiteVerify _siteVerify;

    public string Result { get; set; }

    public ReCaptchaV3Programmatically(IReCaptchaV3SiteVerify siteVerify)
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
