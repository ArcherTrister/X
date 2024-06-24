// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/xabp
// for more information concerning the license and the contributors participating to this project.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using Microsoft.EntityFrameworkCore.DataEncryption.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using X.EntityFrameworkCore.DataEncryption;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides extensions for the <see cref="ModelBuilder"/>.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Enables encryption on this model using an encryption provider.
    /// </summary>
    /// <param name="modelBuilder">
    /// The <see cref="ModelBuilder"/> instance.
    /// </param>
    /// <param name="encryptionProvider">
    /// The <see cref="IEncryptionProvider"/> to use, if any.
    /// </param>
    /// <returns>
    /// The updated <paramref name="modelBuilder"/>.
    /// </returns>
    public static ModelBuilder UseEncryption(this ModelBuilder modelBuilder, IEncryptionProvider encryptionProvider)
    {
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        if (encryptionProvider is null)
        {
            throw new ArgumentNullException(nameof(encryptionProvider));
        }

        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            IEnumerable<EncryptedProperty> encryptedProperties = GetEntityEncryptedProperties(entityType);

            foreach (EncryptedProperty encryptedProperty in encryptedProperties)
            {
#pragma warning disable EF1001 // Internal EF Core API usage.
                if (encryptedProperty.Property.FindAnnotation(CoreAnnotationNames.ValueConverter) is not null)
                {
                    continue;
                }
#pragma warning restore EF1001 // Internal EF Core API usage.

                ValueConverter converter = GetValueConverter(encryptedProperty.Property.ClrType, encryptionProvider);

                if (converter != null)
                {
                    encryptedProperty.Property.SetValueConverter(converter);
                }
            }
        }

        return modelBuilder;
    }

    private static ValueConverter GetValueConverter(Type propertyType, IEncryptionProvider encryptionProvider)
    {
        if (propertyType == typeof(string))
        {
            return new EncryptionConverter<string, string>(encryptionProvider, StorageFormat.Base64);
        }
        else if (propertyType == typeof(byte[]))
        {
            return new EncryptionConverter<byte[], byte[]>(encryptionProvider, StorageFormat.Binary);
        }

        throw new NotImplementedException($"Type {propertyType.Name} does not support encryption.");
    }

    private static IEnumerable<EncryptedProperty> GetEntityEncryptedProperties(IMutableEntityType entity)
    {
        return entity.GetProperties()
            .Select(x => EncryptedProperty.Create(x))
            .Where(x => x is not null);
    }

    internal class EncryptedProperty
    {
        public IMutableProperty Property { get; }

        private EncryptedProperty(IMutableProperty property)
        {
            Property = property;
        }

        public static EncryptedProperty Create(IMutableProperty property)
        {
            var encryptedAttribute = property.PropertyInfo?.GetCustomAttribute<EncryptedAttribute>(false);

            if (encryptedAttribute != null)
            {
                return new EncryptedProperty(property);
            }

            IAnnotation encryptedAnnotation = property.FindAnnotation(PropertyAnnotations.IsEncrypted);

            if (encryptedAnnotation != null && (bool)encryptedAnnotation.Value == true)
            {
                return new EncryptedProperty(property);
            }

            return null;
        }
    }
}
