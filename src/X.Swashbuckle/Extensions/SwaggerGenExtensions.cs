// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using Swashbuckle.AspNetCore.SwaggerGen;

using X.Swashbuckle.Filters.Parameters;
using X.Swashbuckle.Filters.Schemas;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerGenExtensions
    {
        /// <summary>
        /// 显示枚举描述
        /// </summary>
        /// <param name="options">Swagger生成选项</param>
        public static void ShowEnumDescription(this SwaggerGenOptions options)
        {
            options.UseInlineDefinitionsForEnums();
            if (!options.SchemaFilterDescriptors.Exists(x => x.Type == typeof(EnumDescriptionSchemaFilter)))
            {
                options.SchemaFilter<EnumDescriptionSchemaFilter>();
            }

            if (!options.ParameterFilterDescriptors.Exists(x => x.Type == typeof(EnumDescriptionsParameterFilter)))
            {
                options.ParameterFilter<EnumDescriptionsParameterFilter>();
            }
        }
    }
}
