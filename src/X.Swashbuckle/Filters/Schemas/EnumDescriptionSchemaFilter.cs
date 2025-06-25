// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace X.Swashbuckle.Filters.Schemas;

/// <summary>
/// 枚举描述 过滤器。支持Body参数内容
/// </summary>
internal class EnumDescriptionSchemaFilter : EnumHandleBase, ISchemaFilter
{
    /// <summary>
    /// 重写操作处理
    /// </summary>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;
        if (!type.IsEnum)
        {
            return;
        }

        schema.Description = FormatDescription(schema.Description, type);
    }
}
