// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace X.Captcha.H.TagHelpers;

/// <summary>
/// https://docs.hcaptcha.com/configuration.
/// </summary>
[HtmlTargetElement("h-captcha-v2-div", TagStructure = TagStructure.WithoutEndTag)]
public class HCaptchaV2DivTagHelper : TagHelper
{
    /// <summary>
    /// light | dark.
    /// </summary>
    public string Theme { get; set; }

    /// <summary>
    /// normal | compact
    /// </summary>
    public string Size { get; set; }

    public string TabIndex { get; set; }

    public string Callback { get; set; }

    public string ExpiredCallback { get; set; }

    public string ChalexpiredCallback { get; set; }

    public string OpenCallback { get; set; }

    public string CloseCallback { get; set; }

    public string ErrorCallback { get; set; }

    protected CaptchaOptions Options { get; }

    public HCaptchaV2DivTagHelper(IOptionsSnapshot<CaptchaOptions> optionsAccessor)
    {
        Options = optionsAccessor.Get(CaptchaConsts.H2);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="h-captcha"
           data-sitekey="_your_site_key_"
           data-callback="onSubmit"
           data-size="normal">
           ....
        </div>
        */

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Add("class", "h-captcha");
        output.Attributes.Add("data-sitekey", Options.SiteKey);

        if (!string.IsNullOrWhiteSpace(Theme))
        {
            output.Attributes.Add("data-theme", Theme);
        }

        if (!string.IsNullOrWhiteSpace(Size))
        {
            output.Attributes.Add("data-size", Size);
        }

        if (!string.IsNullOrWhiteSpace(TabIndex))
        {
            output.Attributes.Add("data-tabindex", TabIndex);
        }

        if (!string.IsNullOrWhiteSpace(Callback))
        {
            output.Attributes.Add("data-callback", Callback);
        }

        if (!string.IsNullOrWhiteSpace(ExpiredCallback))
        {
            output.Attributes.Add("data-expired-callback", ExpiredCallback);
        }

        if (!string.IsNullOrWhiteSpace(ChalexpiredCallback))
        {
            output.Attributes.Add("data-chalexpired-callback", ChalexpiredCallback);
        }

        if (!string.IsNullOrWhiteSpace(OpenCallback))
        {
            output.Attributes.Add("data-open-callback", OpenCallback);
        }

        if (!string.IsNullOrWhiteSpace(CloseCallback))
        {
            output.Attributes.Add("data-close-callback", CloseCallback);
        }

        if (!string.IsNullOrWhiteSpace(ErrorCallback))
        {
            output.Attributes.Add("data-error-callback", ErrorCallback);
        }
    }
}
