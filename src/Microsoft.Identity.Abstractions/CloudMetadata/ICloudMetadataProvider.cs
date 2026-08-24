// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Resolves cloud-specific metadata for an Azure cloud, keyed by an authority host
    /// (for example <c>login.microsoftonline.com</c> or <c>login.microsoftonline.us</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the dependency-injection seam that lets one SDK contribute cloud-specific values that another
    /// SDK consumes, without either depending on the other's concrete types. The values are carried as a
    /// plain case-insensitive dictionary of strings keyed by the well-known literals in
    /// <see cref="CloudMetadataKeyNames"/>, so new keys can be added without changing this contract or
    /// breaking existing consumers.
    /// </para>
    /// <para>
    /// Implementations should be thread-safe and return quickly, as they may be consulted on the token
    /// acquisition hot path. Use <see cref="InMemoryCloudMetadataProvider"/> for a ready-made mutable
    /// implementation.
    /// </para>
    /// </remarks>
    public interface ICloudMetadataProvider
    {
        /// <summary>
        /// Gets the cloud-specific metadata for the cloud that owns <paramref name="authorityHost"/>.
        /// </summary>
        /// <param name="authorityHost">The authority host (for example <c>login.microsoftonline.us</c>). This
        /// is a bare, non-null host, not a full URL.</param>
        /// <returns>A case-insensitive, read-only dictionary of cloud-specific values (keyed by
        /// <see cref="CloudMetadataKeyNames"/>) for that cloud, or <c>null</c> if the host is unknown to this
        /// provider (allowing a caller to fall back to another provider or a default). An empty
        /// <paramref name="authorityHost"/> also resolves to <c>null</c> rather than throwing.</returns>
        IReadOnlyDictionary<string, string>? GetByAuthorityHost(string authorityHost);
    }
}
