// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace X.Captcha.Re.TagHelpers;

[HtmlTargetElement("re-captcha-v3-script-js", TagStructure = TagStructure.WithoutEndTag)]
public class ReCaptchaV3ScriptJsTagHelper : TagHelper
{
    public string Action { get; set; }

    public string Callback { get; set; }

    public bool Execute { get; set; }

    private readonly CaptchaOptions _options;

    public ReCaptchaV3ScriptJsTagHelper(IOptionsSnapshot<CaptchaOptions> optionsAccessor)
    {
        _options = optionsAccessor.Get(CaptchaConsts.Re3);
        Execute = true;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        myCallback is a user-defined method name or `(function(t){alert(t)})` when Execute = true
        grecaptcha.ready(function () {
            grecaptcha.reExecute = function () {
                grecaptcha.execute('6LccrsMUAAAAANSAh_MCplqdS9AJVPihyzmbPqWa', {
                    action: 'login'
                }).then(function (token) {
                    myCallback(token)
                })
            };
            grecaptcha.reExecute()
        });

        myCallback is a user-defined function when Execute = false
        grecaptcha.ready(function () {
            grecaptcha.reExecute = function (callback) {
                grecaptcha.execute('6LccrsMUAAAAANSAh_MCplqdS9AJVPihyzmbPqWa', {
                    action: 'login'
                }).then(myCallback)
            };
        });
         */
        output.TagName = "script";
        output.TagMode = TagMode.StartTagAndEndTag;

        var script =
            "grecaptcha.ready(function(){ " +
            "grecaptcha.reExecute = function(" + (Execute ? "" : "callback") + "){" +
            "grecaptcha.execute('" + _options.SiteKey + "'" + (string.IsNullOrWhiteSpace(Action) ? "" : ",{action:'" + Action + "'}") + ")" +
            (Execute ? ".then(function(token){" + Callback + "(token)" + "})" : ".then(callback)") + "};" +
            (Execute ? "grecaptcha.reExecute()" : "") +
            "});";
        output.Content.SetHtmlContent(script);
    }
}
