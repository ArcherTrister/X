```csharp

public enum EnumSample
{
    [Description("老大")]
    One = 1,
    [Description("老二")]
    Two = 2,
    [Description("老三")]
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
