// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/XCaptcha
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace X.Captcha.Re;

public class ReCaptchaV2SiteVerify : IReCaptchaV2SiteVerify
{
    private readonly HttpClient _client;
    private readonly CaptchaOptions _options;

    public ReCaptchaV2SiteVerify(IOptionsSnapshot<CaptchaOptions> optionsAccessor, IHttpClientFactory clientFactory)
    {
        _options = optionsAccessor.Get(CaptchaConsts.Re2);
        _client = clientFactory.CreateClient(CaptchaConsts.Re2);
        _client.BaseAddress = new Uri(_options.VerifyBaseUrl);

    }

    public async Task<ReCaptchaV2SiteVerifyResponse> Verify(CaptchaSiteVerifyRequest request)
    {
        using var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("secret", _options.SiteSecret),
            new KeyValuePair<string, string>("response", request.Response),
            new KeyValuePair<string, string>("remoteip", request.RemoteIp),
        });

        var v2Response = await _client.PostAsync("recaptcha/api/siteverify", content);
        if (v2Response.IsSuccessStatusCode)
        {
            /*
            var options = new JsonSerializerOptions();
            options.Converters.Add(new reCAPTCHASiteVerifyResponseJsonConverter());
            return JsonSerializer.Deserialize<reCAPTCHASiteVerifyResponse>(await v2Response.Content.ReadAsStringAsync(), options);
            */
            return JsonSerializer.Deserialize<ReCaptchaV2SiteVerifyResponse>(await v2Response.Content.ReadAsStringAsync());
        }

        return new ReCaptchaV2SiteVerifyResponse
        {
            Success = false,
            ErrorCodes = new[]
            {
                $"http-status-error-{v2Response.StatusCode}",
            },
        };
    }
}
