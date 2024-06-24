using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Masuit.Tools.Systems;

namespace X.Swashbuckle;

/// <summary>
/// 枚举处理基类
/// </summary>
internal abstract class EnumHandleBase
{
    /// <summary>
    /// 格式化描述
    /// </summary>
    /// <param name="type">枚举类型</param>
    protected virtual string FormatDescription(Type type)
    {
        /*
        var sb = new StringBuilder();
        var result = Enum.GetDescriptions(type);
        foreach (var item in result)
            sb.Append($"{item.Value} = {(string.IsNullOrEmpty(item.Description) ? item.Name : item.Description)}{Environment.NewLine}");
        return sb.ToString();
        */
        var enums = Enum.GetNames(type);
        var enumDescriptions = new List<string>();
        foreach (var item in enums)
        {
            var value = (Enum)Enum.Parse(type, item);

            var valueDesc = value.GetDescription();

            enumDescriptions.Add($"{item} = {(string.IsNullOrEmpty(valueDesc) ? item : valueDesc)}");
        }

        return string.Join(Environment.NewLine, enumDescriptions);
    }

    /// <summary>
    /// 枚举前缀
    /// </summary>
    // public const string EnumPrefix = "<p>枚举值：</p>";
    private static readonly CompositeFormat EnumPrefix = CompositeFormat.Parse("<p>枚举值：</p>");

    /// <summary>
    /// 枚举项格式化
    /// </summary>
    /// <remarks>
    /// 0 : 值, 1 : 名称, 2 : 描述
    /// </remarks>
    // public const string EnumItemFormat = "<b>{0} - {1}</b>: {2}";
    private static readonly CompositeFormat EnumItemFormat = CompositeFormat.Parse("<b>{0} - {1}</b>: {2}");

    /// <summary>
    /// 格式化描述
    /// </summary>
    /// <param name="description">描述</param>
    /// <param name="type">枚举类型</param>
    protected virtual string FormatDescription(string description, Type type)
    {
        /*
        var sb = new StringBuilder(description);
        sb.Append(EnumPrefix);
        sb.AppendLine("<ul>");
        foreach (var item in Enum.GetDescriptions(type))
        {
            var itemDesc = string.Format(EnumItemFormat, item.Value, item.Name, item.Description);
            sb.AppendLine($"<li>{itemDesc}</li>");
        }

        sb.AppendLine("</ul>");
        return sb.ToString();
        */
        var enums = Enum.GetNames(type);
        var enumDescriptions = new List<string>();
        foreach (var item in enums)
        {
            var value = (Enum)Enum.Parse(type, item);

            // enumDescriptions.Add($"{item}({valueDesc})={Convert.ToInt32(value, CultureInfo.InvariantCulture)}");
            var itemDesc = string.Format(CultureInfo.InvariantCulture, EnumItemFormat, Convert.ToInt32(value, CultureInfo.InvariantCulture), item, value.GetDescription());
            enumDescriptions.Add($"<li>{itemDesc}</li>");
        }

        // return $"<br><div>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}</div>";
        return $"<ul>{string.Join(Environment.NewLine, enumDescriptions)}</ul>";
    }
}
