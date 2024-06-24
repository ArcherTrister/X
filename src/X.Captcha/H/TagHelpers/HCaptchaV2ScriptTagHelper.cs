// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace X.Captcha.H.TagHelpers;

[HtmlTargetElement("h-captcha-v2-script", TagStructure = TagStructure.WithoutEndTag)]
public class HCaptchaV2ScriptTagHelper : TagHelper
{
    public bool ScriptAsync { get; set; } = true;

    public bool ScriptDefer { get; set; } = true;

    public string Onload { get; set; }

    public string Render { get; set; }

    protected ICaptchaLanguageCodeProvider CaptchaLanguageCodeProvider { get; }

    public HCaptchaV2ScriptTagHelper(ICaptchaLanguageCodeProvider captchaLanguageCodeProvider)
    {
        CaptchaLanguageCodeProvider = captchaLanguageCodeProvider;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
            <script src="https://js.hcaptcha.com/1/api.js" async defer></script>
        */

        output.TagName = "script";
        output.TagMode = TagMode.StartTagAndEndTag;

        var src = $"https://js.hcaptcha.com/1/api.js?" +
                  $"hl={CaptchaLanguageCodeProvider.GetLanguageCode()}";
        if (!string.IsNullOrWhiteSpace(Onload))
        {
            src += $"&onload={Onload}";
        }

        if (!string.IsNullOrWhiteSpace(Render))
        {
            src += $"&render={Render}";
        }

        output.Attributes.Add(new TagHelperAttribute("src", new HtmlString(src)));

        if (ScriptAsync)
        {
            output.Attributes.Add(new TagHelperAttribute("async"));
        }

        if (ScriptDefer)
        {
            output.Attributes.Add(new TagHelperAttribute("defer"));
        }
    }
}
