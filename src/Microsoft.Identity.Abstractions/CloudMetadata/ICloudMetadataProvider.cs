// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Resolves cloud-specific <see cref="CloudMetadata"/> for an Azure cloud, keyed by an authority host
    /// (for example <c>login.microsoftonline.com</c> or <c>login.microsoftonline.us</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the dependency-injection seam that lets one SDK contribute cloud-specific values that another
    /// SDK consumes, without either depending on the other's concrete types. For example MISE can register an
    /// implementation that knows about internal-only sovereign clouds, and Microsoft.Identity.Web resolves it
    /// from the shared container to obtain the correct FIC token-exchange audience for those clouds.
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
        /// is a bare host, not a full URL.</param>
        /// <returns>The <see cref="CloudMetadata"/> for that cloud, or <c>null</c> if the host is unknown to
        /// this provider (allowing a caller to fall back to another provider or a default). A <c>null</c> or
        /// empty <paramref name="authorityHost"/> also resolves to <c>null</c> rather than throwing.</returns>
        CloudMetadata? GetByAuthorityHost(string authorityHost);
    }
}
