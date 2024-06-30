using Microsoft.EntityFrameworkCore;

namespace X.EntityFrameworkCore.FieldEncryption.Demo;

public class DatabaseContext : DbContext
{
    private readonly IEncryptionProvider _encryptionProvider;

    public DbSet<FluentUserEntity> FluentUsers { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IEncryptionProvider encryptionProvider)
        : base(options)
    {
        _encryptionProvider = encryptionProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FluentUserEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            b.Property(x => x.FirstName).IsRequired();
            b.Property(x => x.LastName).IsRequired();
            b.Property(x => x.Email).IsRequired().IsEncrypted();
            b.Property(x => x.Notes).IsRequired().HasColumnType("BLOB").IsEncrypted();
            b.Property(x => x.EncryptedData).IsRequired().IsEncrypted();
            b.Property(x => x.EncryptedDataAsString).IsRequired().HasColumnType("TEXT").IsEncrypted();
        });

        if (_encryptionProvider is not null)
        {
            modelBuilder.UseEncryption(_encryptionProvider);
        }

        base.OnModelCreating(modelBuilder);
    }
}
