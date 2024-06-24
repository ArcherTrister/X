using System.ComponentModel;

namespace X.Swashbuckle.Demo.Models;

/// <summary>
/// 枚举例子
/// </summary>
public enum EnumSample
{
    /// <summary>
    /// 老大
    /// </summary>
    [Description("老大")]
    One = 1,
    /// <summary>
    /// 老二
    /// </summary>
    [Description("老二")]
    Two = 2,
    /// <summary>
    /// 老三
    /// </summary>
    [Description("老三")]
    Three = 3
}
