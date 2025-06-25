// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace X.Captcha.L.TagHelpers;

/// <summary>
/// https://luosimao.com/docs/api/56
/// </summary>
[HtmlTargetElement("l-captcha-v2-div", TagStructure = TagStructure.WithoutEndTag)]
public class LCaptchaV2DivTagHelper : TagHelper
{
    /// <summary>
    /// 组件宽度
    /// 400 如不设置，则按照默认状态显示，如需自适应宽度，可以设置为100%
    /// </summary>
    public string Width { get; set; }

    public string Callback { get; set; }

    protected CaptchaOptions Options { get; }

    public LCaptchaV2DivTagHelper(IOptionsSnapshot<CaptchaOptions> optionsAccessor)
    {
        Options = optionsAccessor.Get(CaptchaConsts.L2);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
         <div class="l-captcha" data-site-key="aab758dfe65d67a418589e950ea07b05"></div>
        */

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Add("class", "l-captcha");
        output.Attributes.Add("data-site-key", Options.SiteKey);

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
