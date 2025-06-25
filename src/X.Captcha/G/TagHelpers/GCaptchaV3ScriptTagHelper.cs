// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace X.Captcha.G.TagHelpers;

[HtmlTargetElement("g-captcha-v3-script", TagStructure = TagStructure.WithoutEndTag)]
public class GCaptchaV3ScriptTagHelper : TagHelper
{
    private readonly CaptchaOptions _options;

    protected ICaptchaLanguageCodeProvider CaptchaLanguageCodeProvider { get; }

    public GCaptchaV3ScriptTagHelper(IOptionsSnapshot<CaptchaOptions> optionsAccessor,
        ICaptchaLanguageCodeProvider captchaLanguageCodeProvider)
    {
        _options = optionsAccessor.Get(CaptchaConsts.G3);
        CaptchaLanguageCodeProvider = captchaLanguageCodeProvider;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
            <script src="https://www.google.com/recaptcha/api.js?render=_reCAPTCHA_site_key"></script>
        */

        output.TagName = "script";
        output.TagMode = TagMode.StartTagAndEndTag;

        var src = $"{_options.VerifyBaseUrl.RemovePostFix(StringComparison.OrdinalIgnoreCase, "/")}/recaptcha/api.js?hl={CaptchaLanguageCodeProvider.GetLanguageCode()}&render={_options.SiteKey}";

        output.Attributes.Add(new TagHelperAttribute("src", new HtmlString(src)));
    }
}
