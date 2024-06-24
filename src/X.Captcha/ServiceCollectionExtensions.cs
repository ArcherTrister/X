// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;

using X.Captcha.G;
using X.Captcha.H;
using X.Captcha.L;
using X.Captcha.Re;

namespace X.Captcha;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGCaptchaV3(
        this IServiceCollection services,
        Action<CaptchaOptions> setupAction,
        Action<HttpClient> configureClient = null)
    {
        if (setupAction != null)
        {
            services.Configure(CaptchaConsts.G3, setupAction);
        }

        if (configureClient == null)
        {
            services.AddHttpClient(CaptchaConsts.G3);
        }
        else
        {
            services.AddHttpClient(CaptchaConsts.G3, configureClient);
        }

        services.AddTransient<ICaptchaLanguageCodeProvider, CultureInfoCaptchaLanguageCodeProvider>();
        services.AddTransient<IGCaptchaV3SiteVerify, GCaptchaV3SiteVerify>();

        return services;
    }

    public static IServiceCollection AddGCaptchaV2(
        this IServiceCollection services,
        Action<CaptchaOptions> setupAction,
        Action<HttpClient> configureClient = null)
    {
        if (setupAction != null)
        {
            services.Configure(CaptchaConsts.G2, setupAction);
        }

        if (configureClient == null)
        {
            services.AddHttpClient(CaptchaConsts.G2);
        }
        else
        {
            services.AddHttpClient(CaptchaConsts.G2, configureClient);
        }

        services.AddTransient<ICaptchaLanguageCodeProvider, CultureInfoCaptchaLanguageCodeProvider>();
        services.AddTransient<IGCaptchaV2SiteVerify, GCaptchaV2SiteVerify>();

        return services;
    }

    public static IServiceCollection AddHCaptchaV2(
        this IServiceCollection services,
        Action<CaptchaOptions> setupAction,
        Action<HttpClient> configureClient = null)
    {
        if (setupAction != null)
        {
            services.Configure(CaptchaConsts.H2, setupAction);
        }

        if (configureClient == null)
        {
            services.AddHttpClient(CaptchaConsts.H2);
        }
        else
        {
            services.AddHttpClient(CaptchaConsts.H2, configureClient);
        }

        services.AddTransient<ICaptchaLanguageCodeProvider, CultureInfoCaptchaLanguageCodeProvider>();
        services.AddTransient<IHCaptchaV2SiteVerify, HCaptchaV2SiteVerify>();

        return services;
    }

    public static IServiceCollection AddLCaptchaV2(
        this IServiceCollection services,
        Action<CaptchaOptions> setupAction,
        Action<HttpClient> configureClient = null)
    {
        if (setupAction != null)
        {
            services.Configure(CaptchaConsts.L2, setupAction);
        }

        if (configureClient == null)
        {
            services.AddHttpClient(CaptchaConsts.L2);
        }
        else
        {
            services.AddHttpClient(CaptchaConsts.L2, configureClient);
        }

        services.AddTransient<ILCaptchaV2SiteVerify, LCaptchaV2SiteVerify>();

        return services;
    }

    public static IServiceCollection AddReCaptchaV3(
    this IServiceCollection services,
    Action<CaptchaOptions> setupAction,
    Action<HttpClient> configureClient = null)
    {
        if (setupAction != null)
        {
            services.Configure(CaptchaConsts.Re3, setupAction);
        }

        if (configureClient == null)
        {
            services.AddHttpClient(CaptchaConsts.Re3);
        }
        else
        {
            services.AddHttpClient(CaptchaConsts.Re3, configureClient);
        }

        services.AddTransient<ICaptchaLanguageCodeProvider, CultureInfoCaptchaLanguageCodeProvider>();
        services.AddTransient<IReCaptchaV3SiteVerify, ReCaptchaV3SiteVerify>();

        return services;
    }

    public static IServiceCollection AddReCaptchaV2(
        this IServiceCollection services,
        Action<CaptchaOptions> setupAction,
        Action<HttpClient> configureClient = null)
    {
        if (setupAction != null)
        {
            services.Configure(CaptchaConsts.Re2, setupAction);
        }

        if (configureClient == null)
        {
            services.AddHttpClient(CaptchaConsts.Re2);
        }
        else
        {
            services.AddHttpClient(CaptchaConsts.Re2, configureClient);
        }

        services.AddTransient<ICaptchaLanguageCodeProvider, CultureInfoCaptchaLanguageCodeProvider>();
        services.AddTransient<IReCaptchaV2SiteVerify, ReCaptchaV2SiteVerify>();

        return services;
    }
}
