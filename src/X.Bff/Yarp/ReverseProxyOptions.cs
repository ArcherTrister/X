// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System.Collections.Generic;
using Yarp.ReverseProxy.Configuration;

namespace X.Bff.Yarp;

public class ReverseProxyOptions
{
    public Dictionary<string, RouteConfig> Routes { get; set; } = new();

    public Dictionary<string, ClusterConfig> Clusters { get; set; } = new();
}
