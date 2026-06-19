// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.Identity.Abstractions.OperationResult.Tests
{
    public class OperationResultTests
    {
        [Fact]
        public void ValidResult()
        {
            var expected = "success";
            var result = new OperationResult<string, OperationError>(expected);

            Assert.True(result.Succeeded);
            Assert.Equal(expected, result.Result);
            Assert.Null(result.Error);
        }

        [Fact]
        public void ValidResultValueType()
        {
            var result = new OperationResult<bool, OperationError>(true);

            Assert.True(result.Succeeded);
            Assert.True(result.Result);
            Assert.Null(result.Error);
        }

        [Fact]
        public void ErrorResult()
        {
            var expectedError = new CustomError();
            var result = new OperationResult<string, OperationError>(expectedError);

            Assert.False(result.Succeeded);
            Assert.Equal(expectedError, result.Error);
            Assert.Null(result.Result);
        }

        [Fact]
        public void ErrorResultValueType()
        {
            var expectedError = new CustomError();
            var result = new OperationResult<bool, OperationError>(expectedError);

            Assert.False(result.Succeeded);
            Assert.Equal(expectedError, result.Error);
            Assert.Equal(default, result.Result);
        }

        [Fact]
        public void SuccessAndError()
        {
            var success = new OperationResult<string, OperationError>("42");
            Assert.True(success.Succeeded);
            Assert.Equal("42", success.Result);
            Assert.Null(success.Error);

            var error = new OperationResult<string, OperationError>(new CustomError());
            Assert.False(error.Succeeded);
            Assert.NotNull(error.Error);
            Assert.Null(error.Result);
        }

        private class CustomError : OperationError
        {
            public string ErrorMessage { get; set; } = "Default error message";
        }
    }
}
