// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/ArcherTrister/xabp
// for more information concerning the license and the contributors participating to this project.

namespace System.ComponentModel.DataAnnotations;

/// <summary>
/// Specifies that the data field value should be encrypted.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class EncryptedAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EncryptedAttribute"/> class.
    /// </summary>
    /// The storage format.
    /// </param>
    public EncryptedAttribute()
    {
    }
}
