// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NET10_0_OR_GREATER
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Provides extension properties for <see cref="CredentialDescription"/> instances (.NET 10+ only).
    /// These extension properties provide property-style access to Certificate and CachedValue
    /// while keeping them hidden from AOT/NativeAOT configuration binders.
    /// </summary>
    /// <remarks>
    /// This uses C# 15 extension property syntax. The extension block defines properties that appear
    /// as instance properties on CredentialDescription but are not part of the type's public API surface
    /// visible to reflection-based tools like configuration binders.
    /// </remarks>
    public static class CredentialDescriptionExtensions
    {
        // C# 15 extension block syntax - defines extension properties on CredentialDescription
        extension(CredentialDescription credential)
        {
            /// <summary>
            /// When <see cref="CredentialDescription.SourceType"/> is <see cref="CredentialSource.Certificate"/>, you will use this property to provide the certificate yourself.
            /// When <see cref="CredentialDescription.SourceType"/> is <see cref="CredentialSource.Base64Encoded"/> or <see cref="CredentialSource.KeyVault"/>
            /// or <see cref="CredentialSource.Path"/> or <see cref="CredentialSource.StoreWithDistinguishedName"/> or <see cref="CredentialSource.StoreWithThumbprint"/>
            /// after the certificate is retrieved by a <see cref="ICredentialsLoader"/>, it will be stored in this property and also in the <see cref="CachedValue"/>.
            /// </summary>
            public X509Certificate2? Certificate
            {
                get => credential.GetCertificateInternal();
                set => credential.SetCertificateInternal(value);
            }

            /// <summary>
            /// When the credential is retrieved by a <see cref="ICredentialsLoader"/>, it will be stored in this property, where you can retrieve it. If the credential is a certificate,
            /// it will also be stored in the <see cref="Certificate"/> property.
            /// </summary>
            public object? CachedValue
            {
                get => credential.GetCachedValueInternal();
                set => credential.SetCachedValueInternal(value);
            }
        }
    }
}
#endif
