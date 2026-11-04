// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions;

/// <summary>
/// Specifies the minimum binding strength for managed identity mTLS proof-of-possession.
/// </summary>
/// <remarks>
/// Names and numeric values align with MSAL's mTLS binding strengths. The value 2 is reserved.
/// A minimum permits stronger binding; it does not select a particular key or disable attestation.
/// </remarks>
public enum MtlsBindingStrength
{
    /// <summary>
    /// Imposes no minimum binding strength. Does not disable proof-of-possession or attestation.
    /// </summary>
    None = 0,

    /// <summary>
    /// Requires at least software-backed binding. Hardware isolation and attestation are not required.
    /// </summary>
    Software = 1,

    /// <summary>
    /// Requires KeyGuard-backed binding with key attestation.
    /// Token acquisition must fail if KeyGuard capability or attestation support is unavailable,
    /// or attestation fails, rather than fall back to weaker binding or a bearer token.
    /// </summary>
    KeyGuard = 3,
}
