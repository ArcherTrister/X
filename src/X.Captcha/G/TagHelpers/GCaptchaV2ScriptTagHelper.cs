// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace X.Captcha.G.TagHelpers;

[HtmlTargetElement("g-captcha-v2-script", TagStructure = TagStructure.WithoutEndTag)]
public class GCaptchaV2ScriptTagHelper : TagHelper
{
    public bool ScriptAsync { get; set; } = true;

    public bool ScriptDefer { get; set; } = true;

    public string Onload { get; set; }

    public string Render { get; set; }

    private readonly CaptchaOptions _options;

    protected ICaptchaLanguageCodeProvider CaptchaLanguageCodeProvider { get; }

    public GCaptchaV2ScriptTagHelper(IOptionsSnapshot<CaptchaOptions> optionsAccessor,
        ICaptchaLanguageCodeProvider captchaLanguageCodeProvider)
    {
        _options = optionsAccessor.Get(CaptchaConsts.G2);
        CaptchaLanguageCodeProvider = captchaLanguageCodeProvider;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
            <script src="https://www.google.com/recaptcha/api.js" async defer></script>
        */

        output.TagName = "script";
        output.TagMode = TagMode.StartTagAndEndTag;

        var src = $"{_options.VerifyBaseUrl.RemovePostFix(StringComparison.OrdinalIgnoreCase, "/")}/recaptcha/api.js?" +
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
