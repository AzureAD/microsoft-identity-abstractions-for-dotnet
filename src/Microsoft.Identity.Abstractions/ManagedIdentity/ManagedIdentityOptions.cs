// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Data object to hold the definition of a managed identity for an application to use for authentication.<br/><br/>
    /// See <see href="https://aka.ms/Entra/ManagedIdentityOverview"/> for more details.
    /// </summary>
    public class ManagedIdentityOptions
    {
        /// <summary>
        /// Gets or sets whether the <see cref="Abstractions.ManagedIdentityType"/> is system-assigned or user assigned.
        /// Defaults to <see cref="ManagedIdentityType.SystemAssigned"/> if not set.<br/><br/>
        /// See <see href="https://aka.ms/Entra/ManagedIdentityOverview"/> for details on these two types of managed identity.
        /// </summary>
        [DefaultValue(ManagedIdentityType.SystemAssigned)]
        public ManagedIdentityType ManagedIdentityType { get; set; }

        /// <summary>
        /// Gets or sets the value of the client id when <see cref="ManagedIdentityType"/> is set to
        /// <see cref="ManagedIdentityType.UserAssigned"/>.<br/>If not set, the default value is null.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Ensures a clone of this object will not have the same reference.
        /// </summary>
        /// <returns>A new instance of <see cref="ManagedIdentityOptions"/> with the same field values.</returns>
        public ManagedIdentityOptions Clone()
        {
            return new ManagedIdentityOptions
            {
                ManagedIdentityType = this.ManagedIdentityType,
                ClientId = this.ClientId
            };
        }

        /// <summary>
        /// Ensures any <see cref="ManagedIdentityOptions"/> object is equal if all fields hold the same values.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ManagedIdentityOptions other = (ManagedIdentityOptions)obj;
            return ManagedIdentityType == other.ManagedIdentityType &&
                ClientId == other.ClientId;
        }

        /// <summary>
        /// Returns the hash code for the instance based on the values of its fields.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return new { ManagedIdentityType, ClientId }.GetHashCode();
        }
    }
}
