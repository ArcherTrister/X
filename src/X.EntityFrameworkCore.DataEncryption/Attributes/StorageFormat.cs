// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/xabp
// for more information concerning the license and the contributors participating to this project.

namespace System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the storage format for an encrypted value.
/// </summary>
public enum StorageFormat
{
    /// <summary>
    /// The value is stored in binary.
    /// </summary>
    Binary,

    /// <summary>
    /// The value is stored in a Base64-encoded string.
    /// </summary>
    /// <remarks>
    /// <b>NB:</b> If the source property is a <see cref="string"/>,
    /// and no encryption provider is configured,
    /// the string will not be modified.
    /// </remarks>
    Base64,
}
