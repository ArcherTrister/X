// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace X.Captcha.G;

public class GCaptchaV2SiteVerify : IGCaptchaV2SiteVerify
{
    private readonly HttpClient _client;
    private readonly CaptchaOptions _options;

    public GCaptchaV2SiteVerify(IOptionsSnapshot<CaptchaOptions> optionsAccessor, IHttpClientFactory clientFactory)
    {
        _options = optionsAccessor.Get(CaptchaConsts.G2);
        _client = clientFactory.CreateClient(CaptchaConsts.G2);
        _client.BaseAddress = new Uri(_options.VerifyBaseUrl);
    }

    public async Task<GCaptchaV2SiteVerifyResponse> Verify(CaptchaSiteVerifyRequest request)
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
            return JsonSerializer.Deserialize<GCaptchaV2SiteVerifyResponse>(await v2Response.Content.ReadAsStringAsync());
        }

        return new GCaptchaV2SiteVerifyResponse
        {
            Success = false,
            ErrorCodes = new[]
            {
                $"http-status-error-{v2Response.StatusCode}",
            },
        };
    }
}
