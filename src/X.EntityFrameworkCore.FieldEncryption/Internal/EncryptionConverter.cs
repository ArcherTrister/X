// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/xabp
// for more information concerning the license and the contributors participating to this project.

using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using X.EntityFrameworkCore.FieldEncryption;

namespace Microsoft.EntityFrameworkCore.DataEncryption.Internal;

/// <summary>
/// Defines the internal encryption converter for string values.
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TProvider"></typeparam>
internal sealed class EncryptionConverter<TModel, TProvider> : ValueConverter<TModel, TProvider>
{
    /// <summary>
    /// Creates a new <see cref="EncryptionConverter{TModel,TProvider}"/> instance.
    /// </summary>
    /// <param name="encryptionProvider">Encryption provider to use.</param>
    /// <param name="storageFormat">Encryption storage format.</param>
    /// <param name="mappingHints">Mapping hints.</param>
    public EncryptionConverter(IEncryptionProvider encryptionProvider, StorageFormat storageFormat, ConverterMappingHints mappingHints = null)
        : base(
            x => Encrypt<TModel, TProvider>(x, encryptionProvider, storageFormat),
            x => Decrypt<TModel, TProvider>(x, encryptionProvider, storageFormat),
            mappingHints)
    {
    }

    private static TOutput Encrypt<TInput, TOutput>(TModel input, IEncryptionProvider encryptionProvider, StorageFormat storageFormat)
    {
        try
        {
            object encryptedData = storageFormat switch
            {
                StorageFormat.Base64 => encryptionProvider.Encrypt(input.ToString()),
                _ => encryptionProvider.Encrypt(input as byte[]),
            };
            return (TOutput)Convert.ChangeType(encryptedData, typeof(TOutput));
        }
        catch (Exception ex)
        {
            return (TOutput)Convert.ChangeType(input, typeof(TOutput));
        }
    }

    private static TModel Decrypt<TInput, TOupout>(TProvider input, IEncryptionProvider encryptionProvider, StorageFormat storageFormat)
    {
        try
        {
            object decryptedData = storageFormat switch
            {
                StorageFormat.Base64 => encryptionProvider.Decrypt(input.ToString()),
                _ => encryptionProvider.Decrypt(input as byte[]),
            };

            return (TModel)Convert.ChangeType(decryptedData, typeof(TModel));
        }
        catch (Exception ex)
        {
            // 记录异常
            return (TModel)Convert.ChangeType(input, typeof(TModel));
        }
    }
}
