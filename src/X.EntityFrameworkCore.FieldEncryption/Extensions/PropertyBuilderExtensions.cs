// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/X
// for more information concerning the license and the contributors participating to this project.

using System;

using Microsoft.EntityFrameworkCore.DataEncryption.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides extensions for the <see cref="PropertyBuilder"/> type.
/// </summary>
public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> IsEncrypted<TProperty>(this PropertyBuilder<TProperty> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasAnnotation(PropertyAnnotations.IsEncrypted, true);

        return builder;
    }

    public static PropertyBuilder IsEncrypted(this PropertyBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasAnnotation(PropertyAnnotations.IsEncrypted, true);

        return builder;
    }
}
