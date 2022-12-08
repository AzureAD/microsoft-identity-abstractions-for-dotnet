// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net.Http;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Specialization of <see cref="DownstreamRestApiOptions"/> where the <see cref="AuthorizationHeaderProviderOptions.HttpMethod"/>
    /// is not settable beyond it's construction.
    /// </summary>
    /// <remarks>This class is useful to provide a better developer experience on the specialized methods
    /// of <see cref="IDownstreamRestApi"/> where the HTTP method is provided already by the name of the 
    /// method, and should not be overriden by the options.</remarks>
    public class DownstreamRestApiOptionsReadOnlyHttpMethod : DownstreamRestApiOptions
    {
        /// <summary>
        /// Copy constructor for <see cref="DownstreamRestApiOptionsReadOnlyHttpMethod"/>.
        /// </summary>
        /// <param name="other">other instance to copy from.</param>
        private DownstreamRestApiOptionsReadOnlyHttpMethod(DownstreamRestApiOptionsReadOnlyHttpMethod other) : base(other)
        {

        }

        /// <summary>
        /// Constructor fro a <see cref="DownstreamRestApiOptions"/> and an HTTP method.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="httpMethod">HTTP method.</param>
        public DownstreamRestApiOptionsReadOnlyHttpMethod(DownstreamRestApiOptions options, HttpMethod httpMethod) : base(options)
        {
            HttpMethod = httpMethod;
        }

        /// <summary>
        /// Http method only readable publicly.
        /// </summary>
        public new HttpMethod HttpMethod { get { return base.HttpMethod; } private set { base.HttpMethod = value; } }

        /// <summary>
        /// Clone the options
        /// </summary>
        /// <returns>A clone.</returns>
        public new DownstreamRestApiOptionsReadOnlyHttpMethod Clone()
        {
            return (CloneInternal() as DownstreamRestApiOptionsReadOnlyHttpMethod)!;
        }

        /// <inheritdoc/>
        protected override AuthorizationHeaderProviderOptions CloneInternal()
        {
            return new DownstreamRestApiOptionsReadOnlyHttpMethod(this);
        }
    }
}
