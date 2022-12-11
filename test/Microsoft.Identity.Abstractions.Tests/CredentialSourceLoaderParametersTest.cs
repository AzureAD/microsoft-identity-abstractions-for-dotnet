// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.Identity.Abstractions.ApplicationOptions.Tests
{
    public class CredentialSourceLoaderParametersTest
    {
        const string clientId = "ClientId-from-app-registration";
        const string authority = "https://login.microsoftonline.com/common/v2.0";
        
        [Fact]
        public void CredentialSourceLoaderParametersProperties()
        {
            CredentialSourceLoaderParameters parameters = new(clientId, authority);

            Assert.Equal(clientId, parameters.ClientId);
            Assert.Equal(authority, parameters.Authority);
        }
    }
}
