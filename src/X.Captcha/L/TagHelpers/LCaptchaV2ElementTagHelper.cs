// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace X.Captcha.L.TagHelpers;

/// <summary>
/// https://luosimao.com/docs/api/56
/// </summary>
[HtmlTargetElement("*", Attributes = WidthAttributeName)]
[HtmlTargetElement("*", Attributes = CallbackAttributeName)]
public class LCaptchaV2ElementTagHelper : TagHelper
{
    private const string WidthAttributeName = "l-captcha-v2-width";
    private const string CallbackAttributeName = "l-captcha-v2-callback";

    [HtmlAttributeName(WidthAttributeName)]
    public string Width { get; set; }

    [HtmlAttributeName(CallbackAttributeName)]
    public string Callback { get; set; }

    private readonly CaptchaOptions _options;

    public LCaptchaV2ElementTagHelper(IOptionsSnapshot<CaptchaOptions> optionsAccessor)
    {
        _options = optionsAccessor.Get(CaptchaConsts.H2);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
         <div class="l-captcha" data-site-key="aab758dfe65d67a418589e950ea07b05"></div>
        */

        output.Attributes.Add("class", "h-captcha");
        output.Attributes.Add("data-sitekey", _options.SiteKey);
        if (!string.IsNullOrWhiteSpace(Width))
        {
            output.Attributes.Add("data-width", Width);
        }

        if (!string.IsNullOrWhiteSpace(Callback))
        {
            output.Attributes.Add("data-callback", Callback);
        }
    }
}
