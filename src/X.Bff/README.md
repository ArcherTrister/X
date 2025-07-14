# BFF (Backend For Frontend) 服务使用说明文档

## 概述
1. BFF 是一种设计模式，旨在为前端应用提供一个定制化的后端服务。本项目通过扩展 ASP.NET Core 的 IServiceCollection 和 IEndpointRouteBuilder 接口，提供了对 BFF 模式的原生支持。本文档将指导您如何在 ASP.NET Core 应用中配置和使用 BFF。

2. 代码和示例是建立在 [OidcProxy.Net](https://github.com/oidcproxydotnet/OidcProxy.Net)、 [Fhi.AuthExtensions](https://github.com/FHIDev/Fhi.AuthExtensions)、 [IdentityServer4](https://github.com/ArcherTrister/IdentityServer4) 等开源项目之上的。

The code and examples are built on top of open-source projects like [OidcProxy.Net](https://github.com/oidcproxydotnet/OidcProxy.Net), [Fhi.AuthExtensions](https://github.com/FHIDev/Fhi.AuthExtensions), [IdentityServer4](https://github.com/ArcherTrister/IdentityServer4), etc.

## 配置与初始化

### 添加 BFF 支持

首先，您需要在 `Startup.cs` 或者 `Program.cs` 文件中的 `ConfigureServices` 方法内调用 `AddBff` 方法来添加 BFF 支持：

```csharp
services.AddBff(config, options =>
{
    // 在这里配置您的 BFFOptions
});
```

- `config`: ReverseProxyOptions 类型的实例，用于配置反向代理。
- `configureOptions`: 一个 `Action<BffOptions>` 委托，用于配置 BFF 相关选项。

此方法会自动注册必要的服务，并配置分布式缓存、身份验证处理程序等。

### 映射 BFF 端点

接下来，在 `Configure` 方法或 `UseEndpoints` 方法中调用 `MapBffEndpoints` 来映射默认的 BFF 端点：


```csharp
app.MapBffEndpoints();
```

or

```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapBffEndpoints();
});
```

这会自动为您映射 `/login`, `/me`, `/logout`, 和 `/back-channel-logout` 端点。

### 注册的端点：

| 端点路径 | HTTP 方法 | 描述 | 认证要求 |
|----------|-----------|------|----------|
| `{prefix}/login` | GET | 登录端点 | 匿名访问 |
| `{prefix}/me` | GET | 用户信息端点 | 匿名访问 |
| `{prefix}/logout` | GET | 登出端点 | 匿名访问 |
| `{prefix}/back-channel-logout` | POST | 后台登出处理 | 匿名访问 |
| - | - | 反向代理端点 | 自动配置 |

> 注意：`{prefix}` 由 `BffOptions.EndpointPrefix` 配置，截止到现在，`Openiddict` 授权框架并未支持 `Backchannel logout`，详情请看此 [issue](https://github.com/openiddict/openiddict-core/issues/2175) 并在其中找到 `SSO logout` 的 [解决方案](https://github.com/openiddict/openiddict-samples/issues/342)。

## 扩展方法

BFF 提供了几个扩展方法以方便进一步定制化：

### AddDefaultCookie

允许您为默认的身份验证方案配置 Cookie 认证选项：

```csharp
bffBuilder.AddDefaultCookie(options =>
{
    // 配置 CookieAuthenticationOptions
});
```

### AddDefaultOpenIdConnect

允许您为 OpenID Connect 身份验证方案进行配置：

```csharp
bffBuilder.AddDefaultOpenIdConnect(options =>
{
    // 配置 OpenIdConnectOptions
});
```

### AddRedisCache

如果您希望使用 Redis 作为分布式缓存而非内存缓存，可以使用该方法：

```csharp
bffBuilder.AddRedisCache(options =>
{
    // 配置 RedisCacheOptions
});
```

## 结论

通过上述步骤，您可以轻松地在您的 ASP.NET Core 应用中集成 BFF 设计模式。这个模式特别适用于微服务架构下的前端应用，它能够有效地简化前端与多个后端服务之间的通信复杂度，同时还能增强安全性。根据您的具体需求，您可以进一步自定义和扩展所提供的功能。
