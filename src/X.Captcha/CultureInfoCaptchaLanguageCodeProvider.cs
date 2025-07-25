﻿// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System.Globalization;

namespace X.Captcha;

public class CultureInfoCaptchaLanguageCodeProvider : ICaptchaLanguageCodeProvider
{
    public string GetLanguageCode()
    {
        return CultureInfo.CurrentUICulture.ToString();
    }
}
