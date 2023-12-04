// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Used by <see cref="ManagedIdentityOptions"/> to specify the type of managed identity to use.
    /// See <see href="https://aka.ms/Entra/ManagedIdentityOverview"/> for more details.
    /// </summary>
    public enum ManagedIdentityType
    {
        /// <summary>
        /// The default value, indicating the managed identity to use is the one configured for the Azure resource on which the
        /// application is running.
        /// </summary>
        SystemAssigned = 0,

        /// <summary>
        /// Indicates the managed identity to use is a user-assigned identity which is defined in a standalone Azure resource.
        /// </summary>
        UserAssigned = 1,
    }
}
