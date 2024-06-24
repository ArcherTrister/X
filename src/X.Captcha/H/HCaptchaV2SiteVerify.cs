// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace X.Captcha.H;

public class HCaptchaV2SiteVerify : IHCaptchaV2SiteVerify
{
    protected CaptchaOptions CaptchaOptions { get; }

    protected IHttpClientFactory HttpClientFactory { get; }

    public HCaptchaV2SiteVerify(IOptionsSnapshot<CaptchaOptions> optionsAccessor, IHttpClientFactory httpClientFactory)
    {
        CaptchaOptions = optionsAccessor.Get(CaptchaConsts.H2);
        HttpClientFactory = httpClientFactory;
    }

    public async Task<HCaptchaV2SiteVerifyResponse> Verify(CaptchaSiteVerifyRequest request)
    {
        Dictionary<string, string> bodyDictionary = new Dictionary<string, string>
        {
            { "secret", CaptchaOptions.SiteSecret },
            { "response", request.Response },
            { "remoteip", request.RemoteIp },
            { "sitekey", CaptchaOptions.SiteKey },
        };

        var client = HttpClientFactory.CreateClient(CaptchaConsts.H2);

        using var content = new FormUrlEncodedContent(bodyDictionary);

        var response = await client.PostAsync(new Uri($"{CaptchaOptions.VerifyBaseUrl}siteverify"), content);
        if (response.IsSuccessStatusCode)
        {
            /*
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CaptchaSiteVerifyResponseJsonConverter());
            return JsonSerializer.Deserialize<HCaptchaV2SiteVerifyResponse>(await response.Content.ReadAsStringAsync(), options);
            */
            return JsonSerializer.Deserialize<HCaptchaV2SiteVerifyResponse>(await response.Content.ReadAsStringAsync());
        }

        return new HCaptchaV2SiteVerifyResponse
        {
            Success = false,
            ErrorCodes = new[]
            {
                $"http-status-error-{response.StatusCode}",
            },
        };
    }
}
