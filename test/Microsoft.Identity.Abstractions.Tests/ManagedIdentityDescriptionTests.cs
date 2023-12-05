// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    public class ManagedIdentityDescriptionTests
    {
        /// <summary>
        /// If no field is set for <see cref="ManagedIdentityOptions"/> the <see cref="ManagedIdentityOptions.ManagedIdentityType"/>
        /// field needs to default to <see cref="ManagedIdentitySource.SystemAssigned"/> as other Microsoft.Identity libraries
        /// will depend on this.
        /// </summary>
        [Fact]
        public void ManagedIdentity_NoDescriptionFieldsSet()
        {
            // Arrange
            ManagedIdentityOptions description = new();

            // Assert
            Assert.Equal(ManagedIdentityType.SystemAssigned, description.ManagedIdentityType);
            Assert.Null(description.ClientId);
        }
    }
}
