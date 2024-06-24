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
