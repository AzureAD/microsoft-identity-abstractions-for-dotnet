// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Data object to hold the definition of a managed identity for an application to use for authentication. If 
    /// <see cref="UserAssignedClientId"/> is null, the application will use the system-assigned managed identity. If
    /// <see cref="UserAssignedClientId"/> is set, the application will try to use the user-assigned managed identity associated
    /// with the provided ClientID. See <see href="https://aka.ms/Entra/ManagedIdentityOverview"/> for more details.
    /// </summary>
    public class ManagedIdentityOptions
    {
        /// <summary>
        /// Gets or sets the value of the ClientID for user-assigned managed identity. If not set, the default value is null
        /// which will tell the application to use the system-assigned managed identity.
        /// </summary>
        [DefaultValue(null)]
        public string? UserAssignedClientId { get; set; }

        /// <summary>
        /// Makes a new object to avoid sharing the same reference.
        /// </summary>
        /// <returns>
        /// New instance of <see cref="ManagedIdentityOptions"/> with the same <see cref="UserAssignedClientId"/>.
        /// </returns>
        public ManagedIdentityOptions Clone()
        {
            return new ManagedIdentityOptions
            {
                UserAssignedClientId = UserAssignedClientId
            };
        }
    }
}
