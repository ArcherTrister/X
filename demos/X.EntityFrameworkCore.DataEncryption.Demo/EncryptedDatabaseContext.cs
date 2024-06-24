using Microsoft.EntityFrameworkCore;

namespace X.EntityFrameworkCore.DataEncryption.Demo;

public class EncryptedDatabaseContext : DatabaseContext
{
    public EncryptedDatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options, null)
    {
    }
}
