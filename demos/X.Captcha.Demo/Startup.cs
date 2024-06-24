using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using X.Captcha;

namespace X.Captcha.Demo;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();

        services.AddGCaptchaV3(x =>
        {
            x.VerifyBaseUrl = "https://recaptcha.google.cn/";
            x.SiteKey = "6LccrsMUAAAAANSAh_MCplqdS9AJVPihyzmbPqWa";
            x.SiteSecret = "6LccrsMUAAAAAL91ysT6Nbhk4MnxpHjyJ_pdVLon";
        });

        services.AddGCaptchaV2(x =>
        {
            x.VerifyBaseUrl = "https://recaptcha.google.cn/";
            x.SiteKey = "6LcArsMUAAAAAKCjwCTktI3GRHTj98LdMEI9f9eQ";
            x.SiteSecret = "6LcArsMUAAAAAO_FBbZghC9aUa1F1rjvcdiOESKd";
        });

        services.AddHCaptchaV2(x =>
        {
            x.VerifyBaseUrl = "https://hcaptcha.com/";
            x.SiteKey = "e0c7cd12-f1de-4d24-9267-658e726688a3";
            x.SiteSecret = "ES_95c2094177dd42d4bf4871060da6b7e8";
        });

        services.AddLCaptchaV2(x =>
        {
            x.VerifyBaseUrl = "https://captcha.luosimao.com/";
            x.SiteKey = "95059ebc73a95c13d50ceb8e06939c94";
            x.SiteSecret = "SiteSecret";
        });

        services.AddReCaptchaV3(x =>
        {
            x.VerifyBaseUrl = "https://recaptcha.net/";
            x.SiteKey = "6Le2mr4ZAAAAAP-83AhlTnJ3jaU9hiwniQ80nI7d";
            x.SiteSecret = "6Le2mr4ZAAAAAGHRZ2INwf1fOtUNwjF8jsHUti2C";
        });

        services.AddReCaptchaV2(x =>
        {
            x.VerifyBaseUrl = "https://recaptcha.net/";
            x.SiteKey = "6LcArsMUAAAAAKCjwCTktI3GRHTj98LdMEI9f9eQ";
            x.SiteSecret = "6LcArsMUAAAAAO_FBbZghC9aUa1F1rjvcdiOESKd";
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
    }
}
