// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System.Net.Http.Headers;
#if NET8_0_OR_GREATER
using Duende.IdentityModel;
#else
using IdentityModel;
#endif
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace X.Bff.Yarp;

    internal class HttpHeaderTransformation : ITransformProvider
    {
        public void ValidateRoute(TransformRouteValidationContext context)
        {
            // I.L.E.
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        {
            // This applies for all clusters
        }

        public void Apply(TransformBuilderContext transformBuilderContext)
        {
            // // apply default YARP logic for forwarding headers
            // transformBuilderContext.CopyRequestHeaders = true;
            //
            // // use YARP default logic for x-forwarded headers
            // transformBuilderContext.UseDefaultForwarders = true;
            //
            // // always remove cookie header since this contains the session
            // transformBuilderContext.RequestTransforms.Add(new RequestHeaderRemoveTransform("Cookie"));
            transformBuilderContext.AddRequestTransform(async transformContext =>
            {
                // var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                // var logger = loggerFactory.CreateLogger(LogCategories.RemoteApiEndpoints);
                var logger = transformContext.HttpContext.RequestServices.GetRequiredService<ILogger<HttpHeaderTransformation>>();

                // var accessToken = await transformContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
                // logger.LogDebug("GetAccessToken: {accessToken}", accessToken);
                // var accessToken = await transformContext.HttpContext.GetManagedAccessToken(tokenType, optional);
                var userToken = await transformContext.HttpContext.GetUserAccessTokenAsync(null);
                logger.LogDebug("GetAccessToken: {accessToken}", userToken.AccessToken);
                transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue(OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer, userToken.AccessToken);
            });
        }
    }
