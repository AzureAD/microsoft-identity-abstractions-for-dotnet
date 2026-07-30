// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Well-known keys for the cloud-specific metadata carried by <see cref="CloudMetadata"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These string literals are the <b>shared vocabulary</b> used to exchange cloud-specific values
    /// between SDKs (for example MISE, Microsoft.Identity.Web, and MSAL). A key literal is intentionally
    /// identical across every SDK that recognizes it, so a value written under a key by one layer can be
    /// read under the same key by another layer without any translation table.
    /// </para>
    /// <para>
    /// The list is <b>add-only</b>: new keys can be introduced without breaking existing callers, because
    /// consumers look up only the keys they understand and ignore the rest. A key that must be retired
    /// should first be marked <see cref="System.ObsoleteAttribute"/> for at least one release before removal.
    /// </para>
    /// </remarks>
    public static class AbstractionsCloudKeys
    {
        /// <summary>
        /// The Federated Identity Credential (FIC) token-exchange audience for a cloud, stored in its
        /// <b>bare</b> form (without a <c>/.default</c> suffix), for example
        /// <c>api://AzureADTokenExchange</c> for the public cloud. Consumers that need the client-credentials
        /// scope form append <c>/.default</c> themselves.
        /// </summary>
        public const string TokenExchangeAudience = "token_exchange_audience";
    }
}
