using Microsoft.EntityFrameworkCore;

#if !USEX
using SoftFluent.EntityFrameworkCore.DataEncryption;
#endif

namespace X.EntityFrameworkCore.FieldEncryption.Demo;

public class DatabaseContext : DbContext
{
#if USEX
    private readonly IFieldEncryptionProvider _fieldEncryptionProvider;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IFieldEncryptionProvider fieldEncryptionProvider)
        : base(options)
    {
        _fieldEncryptionProvider = fieldEncryptionProvider;
    }
#else
    private readonly IEncryptionProvider _fieldEncryptionProvider;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IEncryptionProvider fieldEncryptionProvider)
        : base(options)
    {
        _fieldEncryptionProvider = fieldEncryptionProvider;
    }
#endif

    public DbSet<FluentUserEntity> FluentUsers { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FluentUserEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            b.Property(x => x.FirstName).IsRequired();
            b.Property(x => x.LastName).IsRequired();
            b.Property(x => x.Email).IsRequired().IsEncrypted();
            b.Property(x => x.PhoneNumber).IsRequired().IsEncrypted();
            b.Property(x => x.Notes).IsRequired().HasColumnType("BLOB").IsEncrypted();
            b.Property(x => x.EncryptedData).IsRequired().IsEncrypted();
            b.Property(x => x.EncryptedDataAsString).IsRequired().HasColumnType("TEXT").IsEncrypted();
        });

        if (_fieldEncryptionProvider is not null)
        {
            modelBuilder.UseEncryption(_fieldEncryptionProvider);
        }

        base.OnModelCreating(modelBuilder);
    }
}
