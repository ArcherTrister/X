// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

namespace X.Captcha.H.TagHelpers;

/*
[HtmlTargetElement("hcaptcha-script-v3-js", TagStructure = TagStructure.WithoutEndTag)]
public class HCaptchaV3ScriptJsTagHelper : TagHelper
{
    public string Callback { get; set; }

    public bool Execute { get; set; }

    private readonly hCaptchaOptions _options;

    public HCaptchaV3ScriptJsTagHelper(IOptionsSnapshot<hCaptchaOptions> optionsAccessor)
    {
        _options = optionsAccessor.Get(hCaptchaConsts.HCaptcha);
        Execute = true;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        //var widgetID = hcaptcha.render('captcha-1', { sitekey: 'your_site_key', theme: 'dark' });

        //myCallback is a user-defined method name or `(function(t){alert(t)})` when Execute = true
        //hcaptcha.execute(widgetID, { async: true }).then(({ response, key }) => {myCallback(response, key);});

        //myCallback is a user-defined function when Execute = false
        //hcaptcha.execute(widgetID, { async: true }).then(myCallback);

        output.TagName = "script";
        output.TagMode = TagMode.StartTagAndEndTag;

        var theme = "dark";

        var script = Execute ? $$"""
var hcaptchaWidgetID = hcaptcha.render('captcha-1', { sitekey: '{{_options.SiteKey}}',  theme: '{{theme}}' });
hcaptcha.execute(hcaptchaWidgetID, { async: true }).then(({ response, key }) => { {{Callback}}(response, key); });
""" : $$"""
var hcaptchaWidgetID = hcaptcha.render('captcha-1', { sitekey: '{{_options.SiteKey}}',  theme: '{{theme}}' });
hcaptcha.execute(hcaptchaWidgetID, { async: true }).then({{Callback}});
""";
        output.Content.SetHtmlContent(script);
    }
}
*/
