# X

## X.Captcha

XCaptcha for ASP NET Core

### Install-Package

```
Install-Package X.Captcha
```

### startup

```
        services.AddGCaptchaV3(x =>
        {
            x.VerifyBaseUrl = "https://recaptcha.google.cn/";
            x.SiteKey = "your_site_key";
            x.SiteSecret = "your_site_secret";
        });

        services.AddGCaptchaV2(x =>
        {
            x.VerifyBaseUrl = "https://recaptcha.google.cn/";
            x.SiteKey = "your_site_key";
            x.SiteSecret = "your_site_secret";
        });

        services.AddHCaptchaV2(x =>
        {
            x.VerifyBaseUrl = "https://hcaptcha.com/";
            x.SiteKey = "your_site_key";
            x.SiteSecret = "your_site_secret";
        });

        services.AddLCaptchaV2(x =>
        {
            x.VerifyBaseUrl = "https://captcha.luosimao.com/";
            x.SiteKey = "your_site_key";
            x.SiteSecret = "your_site_secret";
        });

        services.AddReCaptchaV3(x =>
        {
            x.VerifyBaseUrl = "https://recaptcha.net/";
            x.SiteKey = "your_site_key";
            x.SiteSecret = "your_site_secret";
        });

        services.AddReCaptchaV2(x =>
        {
            x.VerifyBaseUrl = "https://recaptcha.net/";
            x.SiteKey = "your_site_key";
            x.SiteSecret = "your_site_secret";
        });
```
