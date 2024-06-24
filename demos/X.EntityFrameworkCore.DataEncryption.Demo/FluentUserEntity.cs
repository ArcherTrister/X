using System;

namespace X.EntityFrameworkCore.DataEncryption.Demo;

public class FluentUserEntity
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Notes { get; set; }

    public byte[] EncryptedData { get; set; }

    public byte[] EncryptedDataAsString { get; set; }
}
