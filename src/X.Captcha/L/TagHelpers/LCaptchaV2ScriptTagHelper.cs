// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace X.Captcha.L.TagHelpers;

[HtmlTargetElement("l-captcha-v2-script", TagStructure = TagStructure.WithoutEndTag)]
public class LCaptchaV2ScriptTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
            <script src="https://captcha.luosimao.com/static/dist/api.js" async defer></script>
        */

        output.TagName = "script";
        output.TagMode = TagMode.StartTagAndEndTag;

        var src = "https://captcha.luosimao.com/static/dist/api.js";

        output.Attributes.Add(new TagHelperAttribute("src", new HtmlString(src)));
    }
}
