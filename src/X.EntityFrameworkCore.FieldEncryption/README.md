## Disclaimer

The code and samples is built on top of [EntityFrameworkCore.DataEncryption](https://github.com/SoftFluent/EntityFrameworkCore.DataEncryption).

<h4 align="center">:warning: This project is **not** affiliated with Microsoft. :warning:</h4><br>

This library has been developed initialy for a personal project of mine which suits my use case. It provides a simple way to encrypt column data.

I **do not** take responsability if you use/deploy this in a production environment and loose your encryption key or corrupt your data.

## How to install

Install the package from [NuGet](https://www.nuget.org/) or from the `Package Manager Console` :

```powershell
PM> Install-Package X.EntityFrameworkCore.FieldEncryption
```

## Supported types

| Type | Default storage type |
|------|----------------------|
| `string` | string |
| `byte[]` | BINARY |

## How to use

`X.EntityFrameworkCore.FieldEncryption` supports 2 differents initialization methods:
* Attribute
* Fluent configuration

Depending on the initialization method you will use, you will need to decorate your `string` or `byte[]` properties of your entities with the `[Encrypted]` attribute or use the fluent `IsEncrypted()` method in your model configuration process.
To use an encryption provider on your EF Core model, and enable the encryption on the `ModelBuilder`.

### Example with `AesFieldEncryptionProvider` and attribute

```csharp
public class UserEntity
{
	public int Id { get; set; }

	[Encrypted]
	public string Username { get; set; }

	[Encrypted]
	public string Password { get; set; }

	public int Age { get; set; }
}

public class DatabaseContext : DbContext
{
	// Get key and IV from a Base64String or any other ways.
	// You can generate a key and IV using "AesFieldEncryptionProvider.GenerateKey()"
	private readonly byte[] _encryptionKey = ...;
	private readonly byte[] _encryptionIV = ...;
	private readonly IFieldEncryptionProvider _provider;

	public DbSet<UserEntity> Users { get; set; }

	public DatabaseContext(DbContextOptions options)
		: base(options)
	{
		_provider = new AesFieldEncryptionProvider(this._encryptionKey, this._encryptionIV);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.UseEncryption(_provider);
	}
}
```
The code bellow creates a new [`AesFieldEncryptionProvider`](https://github.com/ArcherTrister/X.EntityFrameworkCore.FieldEncryption/blob/main/src/EntityFrameworkCore.DataEncryption/Providers/AesFieldEncryptionProvider.cs) and gives it to the current model. It will encrypt every `string` fields of your model that has the `[Encrypted]` attribute when saving changes to database. As for the decrypt process, it will be done when reading the `DbSet<T>` of your `DbContext`.

### Example with `AesFieldEncryptionProvider` and fluent configuration

```csharp
public class UserEntity
{
	public int Id { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public int Age { get; set; }
}

public class DatabaseContext : DbContext
{
	// Get key and IV from a Base64String or any other ways.
	// You can generate a key and IV using "AesFieldEncryptionProvider.GenerateKey()"
	private readonly byte[] _encryptionKey = ...;
	private readonly byte[] _encryptionIV = ...;
	private readonly IFieldEncryptionProvider _provider;

	public DbSet<UserEntity> Users { get; set; }

	public DatabaseContext(DbContextOptions options)
		: base(options)
	{
		_provider = new AesFieldEncryptionProvider(this._encryptionKey, this._encryptionIV);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Entities builder *MUST* be called before UseEncryption().
		var userEntityBuilder = modelBuilder.Entity<UserEntity>();

		userEntityBuilder.Property(x => x.Username).IsRequired().IsEncrypted();
		userEntityBuilder.Property(x => x.Password).IsRequired().IsEncrypted();

		modelBuilder.UseEncryption(_provider);
	}
}
```

## Create an encryption provider

`X.EntityFrameworkCore.FieldEncryption` gives the possibility to create your own encryption providers. To do so, create a new class and make it inherit from `IFieldEncryptionProvider`. You will need to implement the `Encrypt(string)` and `Decrypt(string)` methods.

```csharp
public class MyCustomEncryptionProvider : IFieldEncryptionProvider
{
	public byte[] Encrypt(byte[] input)
	{
		// Encrypt the given input and return the encrypted data as a byte[].
	}

	public byte[] Decrypt(byte[] input)
	{
		// Decrypt the given input and return the decrypted data as a byte[].
	}
}
```

To use it, simply create a new `MyCustomEncryptionProvider` in your `DbContext` and pass it to the `UseEncryption` method:
```csharp
public class DatabaseContext : DbContext
{
	private readonly IFieldEncryptionProvider _provider;

	public DatabaseContext(DbContextOptions options)
		: base(options)
	{
		_provider = new MyCustomEncryptionProvider();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.UseEncryption(_provider);
	}
}
```
