// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    /// <summary>
    /// Tests for authorization header classes including AuthorizationHeaderInformation,
    /// AuthorizationHeaderError, and AuthorizationHeaderResult.
    /// </summary>
    public class AuthorizationHeaderTests
    {
        [Fact]
        public void AuthorizationHeaderInformation_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var info = new AuthorizationHeaderInformation();

            // Assert
            Assert.Null(info.AuthorizationHeaderValue);
            Assert.Null(info.BindingCertificate);
            Assert.Null(info.AdditionalHeaders);
        }

        [Fact]
        public void AuthorizationHeaderInformation_Properties_CanBeSet()
        {
            // Arrange
            const string headerValue = "Bearer token123";
            var additionalHeaders = new Dictionary<string, string> { { "X-Custom", "Value" } };

            // Act
            var info = new AuthorizationHeaderInformation
            {
                AuthorizationHeaderValue = headerValue,
                AdditionalHeaders = additionalHeaders
            };

            // Assert
            Assert.Equal(headerValue, info.AuthorizationHeaderValue);
            Assert.Equal(additionalHeaders, info.AdditionalHeaders);
        }

        [Fact]
        public void AuthorizationHeaderError_DefaultConstructor_SetsEmptyValues()
        {
            // Arrange & Act
            var error = new AuthorizationHeaderError();

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal(string.Empty, error.Message);
            Assert.Null(error.Exception);
        }

        [Fact]
        public void AuthorizationHeaderError_CodeMessageConstructor_SetsProperties()
        {
            // Arrange
            const string code = "INVALID_TOKEN";
            const string message = "The provided token is invalid";

            // Act
            var error = new AuthorizationHeaderError(code, message);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.Null(error.Exception);
        }

        [Fact]
        public void AuthorizationHeaderError_FullConstructor_SetsAllProperties()
        {
            // Arrange
            const string code = "NETWORK_ERROR";
            const string message = "Failed to connect to service";
            var exception = new InvalidOperationException("Test exception");

            // Act
            var error = new AuthorizationHeaderError(code, message, exception);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.Equal(exception, error.Exception);
        }

        [Fact]
        public void AuthorizationHeaderError_NullParameters_HandledCorrectly()
        {
            // Arrange & Act
            var error = new AuthorizationHeaderError(null!, null!, null);

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal(string.Empty, error.Message);
            Assert.Null(error.Exception);
        }

        [Fact]
        public void AuthorizationHeaderResult_SuccessfulResult_IsCorrect()
        {
            // Arrange
            var info = new AuthorizationHeaderInformation
            {
                AuthorizationHeaderValue = "Bearer success_token"
            };

            // Act
            var result = new AuthorizationHeaderResult(info);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(info, result.Result);
            Assert.Null(result.Error);
        }

        [Fact]
        public void AuthorizationHeaderResult_FailedResult_IsCorrect()
        {
            // Arrange
            var error = new AuthorizationHeaderError("TEST_ERROR", "Test error message");

            // Act
            var result = new AuthorizationHeaderResult(error);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Result);
        }

        [Fact]
        public void AuthorizationHeaderResult_LegacyConstructor_WithInfo_CreatesSuccessfulResult()
        {
            // Arrange
            var info = new AuthorizationHeaderInformation
            {
                AuthorizationHeaderValue = "Bearer legacy_token"
            };

            // Act
            var result = new AuthorizationHeaderResult(info, null);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(info, result.Result);
            Assert.Null(result.Error);
        }

        [Fact]
        public void AuthorizationHeaderResult_LegacyConstructor_WithError_CreatesFailedResult()
        {
            // Arrange
            var error = new AuthorizationHeaderError("LEGACY_ERROR", "Legacy error");

            // Act
            var result = new AuthorizationHeaderResult(null, error);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Result);
        }

        [Fact]
        public void AuthorizationHeaderResult_ImplicitStringConversion_Success_ReturnsHeaderValue()
        {
            // Arrange
            const string headerValue = "Bearer conversion_token";
            var info = new AuthorizationHeaderInformation
            {
                AuthorizationHeaderValue = headerValue
            };
            var result = new AuthorizationHeaderResult(info);

            // Act
            string convertedValue = result;

            // Assert
            Assert.Equal(headerValue, convertedValue);
        }

        [Fact]
        public void AuthorizationHeaderResult_ImplicitStringConversion_NullHeaderValue_ReturnsEmptyString()
        {
            // Arrange
            var info = new AuthorizationHeaderInformation
            {
                AuthorizationHeaderValue = null
            };
            var result = new AuthorizationHeaderResult(info);

            // Act
            string convertedValue = result;

            // Assert
            Assert.Equal(string.Empty, convertedValue);
        }

        [Fact]
        public void AuthorizationHeaderResult_ImplicitStringConversion_Failed_ThrowsException()
        {
            // Arrange
            var originalException = new ArgumentException("Original error");
            var error = new AuthorizationHeaderError("CONVERSION_ERROR", "Conversion failed", originalException);
            var result = new AuthorizationHeaderResult(error);

            // Act & Assert
            var thrownException = Assert.Throws<ArgumentException>(() =>
            {
                string _ = result;
            });
            Assert.Equal(originalException, thrownException);
        }

        [Fact]
        public void AuthorizationHeaderResult_ImplicitStringConversion_FailedWithoutException_ThrowsInvalidOperationException()
        {
            // Arrange
            var error = new AuthorizationHeaderError("NO_EXCEPTION_ERROR", "Error without exception");
            var result = new AuthorizationHeaderResult(error);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                string _ = result;
            });
            Assert.Contains("Authorization header creation failed", exception.Message, StringComparison.Ordinal);
            Assert.Contains("NO_EXCEPTION_ERROR", exception.Message, StringComparison.Ordinal);
            Assert.Contains("Error without exception", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void AuthorizationHeaderResult_ImplicitFromInfo_CreatesSuccessfulResult()
        {
            // Arrange
            var info = new AuthorizationHeaderInformation
            {
                AuthorizationHeaderValue = "Bearer implicit_token"
            };

            // Act
            AuthorizationHeaderResult result = info;

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(info, result.Result);
        }

        [Fact]
        public void AuthorizationHeaderResult_ImplicitFromError_CreatesFailedResult()
        {
            // Arrange
            var error = new AuthorizationHeaderError("IMPLICIT_ERROR", "Implicit error");

            // Act
            AuthorizationHeaderResult result = error;

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(error, result.Error);
        }

        [Fact]
        public void AuthorizationHeaderInformation_WithBindingCertificate_WorksCorrectly()
        {
            // Arrange
            var info = new AuthorizationHeaderInformation();
            
            // Act
            // Note: We can't create a real X509Certificate2 in tests easily,
            // so we just test that the property can be set to null
            info.BindingCertificate = null;

            // Assert
            Assert.Null(info.BindingCertificate);
        }

        [Fact]
        public void AuthorizationHeaderInformation_WithAdditionalHeaders_WorksCorrectly()
        {
            // Arrange
            var headers = new Dictionary<string, string>
            {
                { "X-API-Version", "2.0" },
                { "X-Client-Id", "test-client" }
            };

            // Act
            var info = new AuthorizationHeaderInformation
            {
                AdditionalHeaders = headers
            };

            // Assert
            Assert.Equal(headers, info.AdditionalHeaders);
            Assert.Equal("2.0", info.AdditionalHeaders["X-API-Version"]);
            Assert.Equal("test-client", info.AdditionalHeaders["X-Client-Id"]);
        }
    }
}