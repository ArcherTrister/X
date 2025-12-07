// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

#if NET10_0_OR_GREATER
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
#else
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
#endif

namespace X.Swashbuckle.Providers;

/// <summary>
/// 缓存Swagger
/// </summary>
/// <example>
/// <code>
/// services.Replace(ServiceDescriptor.Transient&lt;ISwaggerProvider, CachingSwaggerProvider&gt;());
/// or
/// services.Replace(ServiceDescriptor.Transient&lt;IAsyncSwaggerProvider, CachingSwaggerProvider&gt;());
/// </code>
/// </example>
public class CachingSwaggerProvider : ISwaggerProvider, IAsyncSwaggerProvider
{
    private static readonly ConcurrentDictionary<string, OpenApiDocument> _cache = new ConcurrentDictionary<string, OpenApiDocument>();

    private readonly SwaggerGenerator _swaggerGenerator;

    public CachingSwaggerProvider(
        IOptions<SwaggerGeneratorOptions> optionsAccessor,
        IApiDescriptionGroupCollectionProvider apiDescriptionsProvider,
        ISchemaGenerator schemaGenerator)
    {
        _swaggerGenerator = new SwaggerGenerator(optionsAccessor.Value, apiDescriptionsProvider, schemaGenerator);
    }

    public OpenApiDocument GetSwagger(string documentName, string host = null, string basePath = null)
    {
        return _cache.GetOrAdd(documentName, (_) => _swaggerGenerator.GetSwagger(documentName, host, basePath));
    }

    public async Task<OpenApiDocument> GetSwaggerAsync(string documentName, string host = null, string basePath = null)
    {
        return _cache.GetOrAdd(documentName, await _swaggerGenerator.GetSwaggerAsync(documentName, host, basePath));
    }
}
