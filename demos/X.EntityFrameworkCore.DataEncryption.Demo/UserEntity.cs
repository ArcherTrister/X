using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X.EntityFrameworkCore.DataEncryption.Demo;

public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [Encrypted]
    public string Email { get; set; }

    [Required]
    [Encrypted]
    public string Notes { get; set; }

    [Required]
    [Encrypted]
    public byte[] EncryptedData { get; set; }

    [Required]
    [Encrypted]
    [Column(TypeName = "TEXT")]
    public byte[] EncryptedDataAsString { get; set; }
}
