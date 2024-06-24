// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace X.Captcha.L;

public class LCaptchaV2SiteVerify : ILCaptchaV2SiteVerify
{
    protected CaptchaOptions CaptchaOptions { get; }

    protected IHttpClientFactory HttpClientFactory { get; }

    public LCaptchaV2SiteVerify(IOptionsSnapshot<CaptchaOptions> optionsAccessor, IHttpClientFactory httpClientFactory)
    {
        CaptchaOptions = optionsAccessor.Get(CaptchaConsts.H2);
        HttpClientFactory = httpClientFactory;
    }

    public async Task<LCaptchaV2SiteVerifyResponse> Verify(CaptchaSiteVerifyRequest request)
    {
        Dictionary<string, string> bodyDictionary = new Dictionary<string, string>
        {
            { "api_key", CaptchaOptions.SiteSecret },
            { "response", request.Response },
        };

        var client = HttpClientFactory.CreateClient(CaptchaConsts.H2);

        using var content = new FormUrlEncodedContent(bodyDictionary);

        var response = await client.PostAsync(new Uri($"{CaptchaOptions.VerifyBaseUrl}api/site_verify"), content);
        if (response.IsSuccessStatusCode)
        {
            /*
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CaptchaSiteVerifyResponseJsonConverter());
            return JsonSerializer.Deserialize<HCaptchaV2SiteVerifyResponse>(await response.Content.ReadAsStringAsync(), options);
            */
            return JsonSerializer.Deserialize<LCaptchaV2SiteVerifyResponse>(await response.Content.ReadAsStringAsync());
        }

        return new LCaptchaV2SiteVerifyResponse
        {
            Success = false,
            ErrorCodes = new[]
            {
                $"http-status-error-{response.StatusCode}",
            },
        };
    }
}
