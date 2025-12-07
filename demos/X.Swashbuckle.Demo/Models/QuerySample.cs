using System.ComponentModel;

namespace X.Swashbuckle.Demo.Models;

/// <summary>
/// 查询例子
/// </summary>
public class QuerySample
{
    /// <summary>
    /// 编号
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [DefaultValue("隔壁老黑")]
    public string Name { get; set; } = "隔壁老王";

    /// <summary>
    /// 密码
    /// </summary>
    // [SwaggerIgnoreProperty]
    public string Password { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [DefaultValue(1)]
    public int Gender { get; set; } = 2;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 枚举例子
    /// </summary>
    [DefaultValue(Models.EnumSample.Two)]
    public EnumSample EnumSample { get; set; }
}
