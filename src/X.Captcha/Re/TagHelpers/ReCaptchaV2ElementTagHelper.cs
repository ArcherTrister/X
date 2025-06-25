// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace X.Captcha.Re.TagHelpers;

[HtmlTargetElement("*", Attributes = BadgeAttributeName)]
[HtmlTargetElement("*", Attributes = ThemeAttributeName)]
[HtmlTargetElement("*", Attributes = SizeAttributeName)]
[HtmlTargetElement("*", Attributes = TabIndexAttributeName)]
[HtmlTargetElement("*", Attributes = CallbackAttributeName)]
[HtmlTargetElement("*", Attributes = ExpiredCallbackAttributeName)]
[HtmlTargetElement("*", Attributes = ErrorCallbackAttributeName)]
public class ReCaptchaV2ElementTagHelper : TagHelper
{
    private const string BadgeAttributeName = "re-captcha-v2-badge";
    private const string ThemeAttributeName = "re-captcha-v2-theme";
    private const string SizeAttributeName = "re-captcha-v2-size";
    private const string TabIndexAttributeName = "re-captcha-v2-tab-index";
    private const string CallbackAttributeName = "re-captcha-v2-callback";
    private const string ExpiredCallbackAttributeName = "re-captcha-v2-expired-callback";
    private const string ErrorCallbackAttributeName = "re-captcha-v2-error-callback";

    [HtmlAttributeName(BadgeAttributeName)]
    public string Badge { get; set; }

    [HtmlAttributeName(ThemeAttributeName)]
    public string Theme { get; set; }

    [HtmlAttributeName(SizeAttributeName)]
    public string Size { get; set; }

    [HtmlAttributeName(TabIndexAttributeName)]
    public string TabIndex { get; set; }

    [HtmlAttributeName(CallbackAttributeName)]
    public string Callback { get; set; }

    [HtmlAttributeName(ExpiredCallbackAttributeName)]
    public string ExpiredCallback { get; set; }

    [HtmlAttributeName(ErrorCallbackAttributeName)]
    public string ErrorCallback { get; set; }

    private readonly CaptchaOptions _options;

    public ReCaptchaV2ElementTagHelper(IOptionsSnapshot<CaptchaOptions> optionsAccessor)
    {
        _options = optionsAccessor.Get(CaptchaConsts.Re2);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="g-recaptcha"
           data-sitekey="_your_site_key_"
           data-callback="onSubmit"
           data-size="invisible">
           ....
        </div>
        */

        output.Attributes.Add("class", "g-recaptcha");
        output.Attributes.Add("data-sitekey", _options.SiteKey);
        if (!string.IsNullOrWhiteSpace(Badge))
        {
            output.Attributes.Add("data-badge", Badge);
        }

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

        if (!string.IsNullOrWhiteSpace(ErrorCallback))
        {
            output.Attributes.Add("data-error-callback", ErrorCallback);
        }
    }
}
