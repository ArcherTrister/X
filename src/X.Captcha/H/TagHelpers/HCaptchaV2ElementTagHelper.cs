// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace X.Captcha.H.TagHelpers;

/// <summary>
/// https://docs.hcaptcha.com/configuration/.
/// </summary>
[HtmlTargetElement("*", Attributes = ThemeAttributeName)]
[HtmlTargetElement("*", Attributes = SizeAttributeName)]
[HtmlTargetElement("*", Attributes = TabIndexAttributeName)]
[HtmlTargetElement("*", Attributes = CallbackAttributeName)]
[HtmlTargetElement("*", Attributes = ExpiredCallbackAttributeName)]
[HtmlTargetElement("*", Attributes = ChalexpiredCallbackAttributeName)]
[HtmlTargetElement("*", Attributes = OpenCallbackAttributeName)]
[HtmlTargetElement("*", Attributes = CloseCallbackAttributeName)]
[HtmlTargetElement("*", Attributes = ErrorCallbackAttributeName)]
public class HCaptchaV2ElementTagHelper : TagHelper
{
    private const string ThemeAttributeName = "h-captcha-v2-theme";
    private const string SizeAttributeName = "h-captcha-v2-size";
    private const string TabIndexAttributeName = "h-captcha-v2-tab-index";
    private const string CallbackAttributeName = "h-captcha-v2-callback";
    private const string ExpiredCallbackAttributeName = "h-captcha-v2-expired-callback";
    private const string ChalexpiredCallbackAttributeName = "h-captcha-v2-chalexpired-callback";
    private const string OpenCallbackAttributeName = "h-captcha-v2-open-callback";
    private const string CloseCallbackAttributeName = "h-captcha-v2-close-callback";
    private const string ErrorCallbackAttributeName = "h-captcha-v2-error-callback";

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

    [HtmlAttributeName(ChalexpiredCallbackAttributeName)]
    public string ChalexpiredCallback { get; set; }

    [HtmlAttributeName(OpenCallbackAttributeName)]
    public string OpenCallback { get; set; }

    [HtmlAttributeName(CloseCallbackAttributeName)]
    public string CloseCallback { get; set; }

    [HtmlAttributeName(ErrorCallbackAttributeName)]
    public string ErrorCallback { get; set; }

    private readonly CaptchaOptions _options;

    public HCaptchaV2ElementTagHelper(IOptionsSnapshot<CaptchaOptions> optionsAccessor)
    {
        _options = optionsAccessor.Get(CaptchaConsts.H2);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="h-captcha"
           data-sitekey="_your_site_key_"
           data-callback="onSubmit"
           data-size="invisible">
           ....
        </div>
        */

        output.Attributes.Add("class", "h-captcha");
        output.Attributes.Add("data-sitekey", _options.SiteKey);
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
