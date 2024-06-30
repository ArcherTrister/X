// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/xabp
// for more information concerning the license and the contributors participating to this project.

namespace X.EntityFrameworkCore.FieldEncryption;

/// <summary>
/// Provides a mechanism to encrypt and decrypt data.
/// </summary>
public interface IEncryptionProvider
{
    string Decrypt(string cipherText);

    byte[] Decrypt(byte[] cipherTextBytes);

    string Encrypt(string plainText);

    byte[] Encrypt(byte[] plainTextBytes);
}
