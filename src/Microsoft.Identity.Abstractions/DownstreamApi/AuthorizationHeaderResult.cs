// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Represents the result of an authorization header creation operation.
    /// This result can contain either successful authorization header information or error details.
    /// </summary>
    /// <remarks>
    /// This class wraps <see cref="OperationResult{TResult, TError}"/> to provide strongly-typed
    /// results for authorization header operations. It includes an implicit conversion to string
    /// for backward compatibility with existing code that expects a string authorization header.
    /// </remarks>
    public class AuthorizationHeaderResult
    {
        private readonly OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError> _operationResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHeaderResult"/> class
        /// with successful authorization header information.
        /// </summary>
        /// <param name="info">The authorization header information containing the result of a successful operation.</param>
        public AuthorizationHeaderResult(AuthorizationHeaderInformation info)
        {
            _operationResult = new OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>(info);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHeaderResult"/> class
        /// with error information.
        /// </summary>
        /// <param name="error">The error information describing why the operation failed.</param>
        public AuthorizationHeaderResult(AuthorizationHeaderError error)
        {
            _operationResult = new OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>(error);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHeaderResult"/> class
        /// with either authorization header information or error details.
        /// </summary>
        /// <param name="info">The authorization header information if the operation succeeded, or null if it failed.</param>
        /// <param name="error">The error information if the operation failed, or null if it succeeded.</param>
        /// <remarks>
        /// Exactly one of <paramref name="info"/> or <paramref name="error"/> should be non-null.
        /// This constructor is provided for compatibility with the original design proposal.
        /// </remarks>
        public AuthorizationHeaderResult(AuthorizationHeaderInformation? info, AuthorizationHeaderError? error)
        {
            if (info is not null)
            {
                _operationResult = new OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>(info);
            }
            else if (error is not null)
            {
                _operationResult = new OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>(error);
            }
            else
            {
                throw new ArgumentException("Either info or error must be provided.");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the operation succeeded.
        /// </summary>
        /// <value>
        /// <c>true</c> if the operation succeeded and <see cref="Result"/> contains a valid value;
        /// <c>false</c> if the operation failed and <see cref="Error"/> contains the error.
        /// </value>
        public bool Succeeded => _operationResult.Succeeded;

        /// <summary>
        /// Gets the authorization header information associated with the operation result.
        /// </summary>
        /// <returns>The authorization header information associated with the operation result.</returns>
        /// <remarks>This property is only valid if <see cref="Succeeded"/> is <c>true</c>.</remarks>
        public AuthorizationHeaderInformation? Result => _operationResult.Result;

        /// <summary>
        /// Gets the error associated with the operation result.
        /// </summary>
        /// <returns>The error associated with the operation result.</returns>
        /// <remarks>This property is only valid if <see cref="Succeeded"/> is <c>false</c>.</remarks>
        public AuthorizationHeaderError? Error => _operationResult.Error;

        /// <summary>
        /// Implicitly converts an <see cref="AuthorizationHeaderResult"/> to a string
        /// containing the authorization header value.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        /// <returns>The authorization header value if the operation succeeded.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the result represents a failed operation. If the error contains an exception,
        /// the original exception is thrown; otherwise, a new <see cref="InvalidOperationException"/>
        /// is thrown with details about the failure.
        /// </exception>
        /// <remarks>
        /// This implicit conversion provides backward compatibility with code that expects
        /// authorization header providers to return string values directly.
        /// </remarks>
        public static implicit operator string(AuthorizationHeaderResult result)
        {
            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (!result.Succeeded)
            {
                if (result.Error?.Exception is not null)
                {
                    throw result.Error.Exception;
                }
                throw new InvalidOperationException($"Authorization header creation failed. Code: {result.Error?.Code}, Message: {result.Error?.Message}");
            }
            return result.Result?.AuthorizationHeaderValue ?? string.Empty;
        }

        /// <summary>
        /// Implicitly creates an <see cref="AuthorizationHeaderResult"/> from
        /// <see cref="AuthorizationHeaderInformation"/>.
        /// </summary>
        /// <param name="info">The authorization header information.</param>
        /// <returns>A successful result containing the authorization header information.</returns>
        public static implicit operator AuthorizationHeaderResult(AuthorizationHeaderInformation info)
        {
            return new AuthorizationHeaderResult(info);
        }

        /// <summary>
        /// Implicitly creates an <see cref="AuthorizationHeaderResult"/> from
        /// <see cref="AuthorizationHeaderError"/>.
        /// </summary>
        /// <param name="error">The authorization header error.</param>
        /// <returns>A failed result containing the error information.</returns>
        public static implicit operator AuthorizationHeaderResult(AuthorizationHeaderError error)
        {
            return new AuthorizationHeaderResult(error);
        }
    }
}