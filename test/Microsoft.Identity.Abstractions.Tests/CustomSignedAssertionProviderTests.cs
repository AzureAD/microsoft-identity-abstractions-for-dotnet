// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    public class CustomSignedAssertionProviderTests
    {
        [Fact]
        public void CustomSignedAssertionProvider_ShouldReturnConfiguredName()
        {
            // Arrange
            string testProvider = "TestProvider";
            var expectedCredentialSource = CredentialSource.CustomSignedAssertion;
            var mockProvider = new MockCustomSignedAssertionProvider(testProvider);
            
            // Act
            var name = mockProvider.Name;
            var credSource = mockProvider.CredentialSource;

            // Assert
            Assert.Equal(testProvider, name);
            Assert.Equal(expectedCredentialSource, credSource);

        }
    }

    internal class MockCustomSignedAssertionProvider(string name) : object(), ICustomSignedAssertionProvider
    {
        public string Name { get; set; } = name;

        public CredentialSource CredentialSource => CredentialSource.CustomSignedAssertion;

        public Task LoadIfNeededAsync(CredentialDescription credentialDescription, CredentialSourceLoaderParameters? parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
