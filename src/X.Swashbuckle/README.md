```csharp

public enum EnumSample
{
    [Description("�ϴ�")]
    One = 1,
    [Description("�϶�")]
    Two = 2,
    [Description("����")]
    Three = 3
}

services.AddSwaggerGen(options =>
{
    ...
    options.ShowEnumDescription();
});

```

```csharp
services.Replace(ServiceDescriptor.Transient<ISwaggerProvider, CachingSwaggerProvider>());
services.Replace(ServiceDescriptor.Transient<IAsyncSwaggerProvider, CachingSwaggerProvider>());
```
