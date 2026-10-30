// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions;

/// <summary>
/// Specifies the key attestation policy for managed identity mTLS token acquisition.
/// </summary>
public enum KeyAttestationMode
{
    /// <summary>
    /// Requires key attestation. Token acquisition must fail if attestation support is unavailable
    /// or attestation fails, rather than fall back to an unattested request.
    /// This is the default policy for managed identity mTLS token acquisition.
    /// </summary>
    Required = 0,

    /// <summary>
    /// Disables key attestation, even when attestation support is registered.
    /// This does not disable mTLS or relax platform or service requirements.
    /// </summary>
    Disabled = 1,
}
