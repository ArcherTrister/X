// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Linq;

namespace X.Captcha;

public static class StringExtensions
{
    public static string RemovePostFix(this string str, StringComparison comparisonType, params string[] postFixes)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        if (postFixes == null || postFixes.Length == 0)
        {
            return str;
        }

        foreach (var postFix in postFixes)
        {
            if (str.EndsWith(postFix, comparisonType))
            {
                return str.Left(str.Length - postFix.Length);
            }
        }

        return str;
    }

    public static string Left(this string str, int len)
    {
        return str.Length < len ? str : str[..len];
    }
}
