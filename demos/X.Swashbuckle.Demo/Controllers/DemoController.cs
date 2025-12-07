using Microsoft.AspNetCore.Mvc;
using X.Swashbuckle.Demo.Models;
using X.Swashbuckle.Demo.Models.Responses;

namespace X.Swashbuckle.Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sample">查询</param>
    [HttpGet("Query")]
    public virtual IActionResult Query([FromQuery] QuerySample sample)
    {
        return new JsonResult(sample);
    }

    /// <summary>
    /// 获取默认值
    /// </summary>
    /// <param name="q">字符串</param>
    /// <param name="page">页索引</param>
    /// <param name="pageSize">每页记录数</param>
    /// <param name="enumSample">枚举例子</param>
    [HttpGet("GetDefaultValue")]
    public virtual IActionResult GetDefaultValue([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] EnumSample enumSample = EnumSample.Two)
    {
        return new JsonResult(new
        {
            q,
            page,
            pageSize
        });
    }

    /// <summary>
    /// 提交
    /// </summary>
    /// <param name="sample">查询例子</param>
    [HttpPost]
    public IActionResult Post([FromBody] QuerySample sample)
    {
        return new JsonResult(sample);
    }

    /// <summary>
    /// 测试 枚举字典响应
    /// </summary>
    [HttpGet("TestEnumDictionaryResp")]
    [ProducesResponseType(typeof(EnumDictionaryResponse), 200)]
    public IActionResult TestEnumDictionaryResp() => new JsonResult(new EnumDictionaryResponse());
}
