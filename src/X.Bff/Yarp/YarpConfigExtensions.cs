// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Yarp.ReverseProxy.Configuration;

namespace X.Bff.Yarp;

public static class YarpConfigExtensions
{
    public static IReadOnlyList<RouteConfig> ToRouteConfig(this Dictionary<string, RouteConfig> source)
        => source.Select(x => x.ToRouteConfig()).ToImmutableArray();

    public static IReadOnlyList<ClusterConfig> ToClusterConfig(this Dictionary<string, ClusterConfig> source)
        => source.Select(x => x.ToClusterConfig()).ToImmutableArray();

    private static RouteConfig ToRouteConfig(this KeyValuePair<string, RouteConfig> source)
    {
        return new RouteConfig
        {
            RouteId = source.Key,
            Match = source.Value.Match,
            Order = source.Value.Order,
            ClusterId = source.Value.ClusterId,
            AuthorizationPolicy = source.Value.AuthorizationPolicy,
            CorsPolicy = source.Value.CorsPolicy,
            MaxRequestBodySize = source.Value.MaxRequestBodySize,
            Metadata = source.Value.Metadata,
            Transforms = source.Value.Transforms,
        };
    }

    private static ClusterConfig ToClusterConfig(this KeyValuePair<string, ClusterConfig> source)
    {
        return new ClusterConfig
        {
            ClusterId = source.Key,
            LoadBalancingPolicy = source.Value.LoadBalancingPolicy,
            SessionAffinity = source.Value.SessionAffinity,
            HealthCheck = source.Value.HealthCheck,
            HttpClient = source.Value.HttpClient,
            HttpRequest = source.Value.HttpRequest,
            Destinations = source.Value.Destinations,
            Metadata = source.Value.Metadata,
        };
    }
}
