// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Represents an error that occurred during authorization header creation or processing.
    /// </summary>
    public class AuthorizationHeaderError : OperationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHeaderError"/> class.
        /// </summary>
        public AuthorizationHeaderError()
        {
            Code = string.Empty;
            Message = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHeaderError"/> class
        /// with the specified error code and message.
        /// </summary>
        /// <param name="code">The error code that identifies the type of error.</param>
        /// <param name="message">A human-readable message describing the error.</param>
        public AuthorizationHeaderError(string code, string message)
        {
            Code = code ?? string.Empty;
            Message = message ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHeaderError"/> class
        /// with the specified error code, message, and underlying exception.
        /// </summary>
        /// <param name="code">The error code that identifies the type of error.</param>
        /// <param name="message">A human-readable message describing the error.</param>
        /// <param name="exception">The underlying exception that caused this error.</param>
        public AuthorizationHeaderError(string code, string message, Exception? exception)
        {
            Code = code ?? string.Empty;
            Message = message ?? string.Empty;
            Exception = exception;
        }

        /// <summary>
        /// Gets or sets the error code that identifies the type of error.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a human-readable message describing the error.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the underlying exception that caused this error, if any.
        /// </summary>
        public Exception? Exception { get; set; }
    }
}