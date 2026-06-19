// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// SDK-to-SDK helpers for flowing the outgoing <see cref="HttpRequestMessage"/> to a request-aware
    /// authorization header provider through <see cref="AcquireTokenOptions.ExtraParameters"/>.
    /// </summary>
    /// <remarks>
    /// Not meant to be used by application developers, but by other SDKs. Some protocols bind the authorization
    /// header to the outgoing request - for example a PoP SignedHttpRequest's <c>q</c>/<c>h</c>/<c>b</c> claims
    /// hash the request's query, headers, and body. The calling SDK sets the request before the header is created;
    /// the header-provider SDK reads it. Using the existing <see cref="AcquireTokenOptions.ExtraParameters"/> bag
    /// avoids adding a new interface for each request-bound protocol (SHR, mTLS PoP, acquisition metadata, ...).
    /// </remarks>
    public static class AcquireTokenOptionsExtensions
    {
        private const string HttpRequestMessageKey = "Microsoft.Identity.Abstractions.HttpRequestMessage";

        /// <summary>
        /// Stores the outgoing <see cref="HttpRequestMessage"/> so a request-aware authorization header provider
        /// can bind to it. The request's content should already be set so that body binding is accurate.
        /// </summary>
        /// <param name="options">The token acquisition options to attach the request to.</param>
        /// <param name="httpRequestMessage">The outgoing request whose material may be bound into the header.</param>
        /// <remarks>
        /// Writes to a fresh copy of <see cref="AcquireTokenOptions.ExtraParameters"/>. <see cref="AcquireTokenOptions.Clone"/>
        /// shallow-copies the dictionary reference, so a per-call options object can share one instance with a
        /// DI-singleton; cloning before writing keeps this per-request value isolated to the current call and prevents
        /// concurrent acquisitions from observing each other's request.
        /// </remarks>
        public static void SetHttpRequestMessage(this AcquireTokenOptions options, HttpRequestMessage httpRequestMessage)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));
            _ = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));

            options.ExtraParameters = options.ExtraParameters == null
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>(options.ExtraParameters);

            options.ExtraParameters[HttpRequestMessageKey] = httpRequestMessage;
        }

        /// <summary>
        /// Gets the outgoing <see cref="HttpRequestMessage"/> previously stored by
        /// <see cref="SetHttpRequestMessage(AcquireTokenOptions, HttpRequestMessage)"/>, or <see langword="null"/>
        /// if none was set. The reader does not own the request; the SDK that created it remains responsible for disposal.
        /// </summary>
        /// <param name="options">The token acquisition options to read the request from.</param>
        /// <returns>The stored request, or <see langword="null"/>.</returns>
        public static HttpRequestMessage? GetHttpRequestMessage(this AcquireTokenOptions options)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));

            if (options.ExtraParameters != null
                && options.ExtraParameters.TryGetValue(HttpRequestMessageKey, out object? value)
                && value is HttpRequestMessage httpRequestMessage)
            {
                return httpRequestMessage;
            }

            return null;
        }
    }
}
