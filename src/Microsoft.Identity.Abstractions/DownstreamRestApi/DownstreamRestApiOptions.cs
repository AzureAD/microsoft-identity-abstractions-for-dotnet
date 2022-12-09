// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options passed-in to call downstream web APIs.
    /// </summary>
    public class DownstreamRestApiOptions : AuthorizationHeaderProviderOptions
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DownstreamRestApiOptions()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public DownstreamRestApiOptions(DownstreamRestApiOptions other) : base(other)
        {
            Scopes = other.Scopes;
            Serializer = other.Serializer;
            Deserializer = other.Deserializer;
        }

        /// <summary>
        /// Clone the options (to be able to override them).
        /// </summary>
        /// <returns>A clone of the options.</returns>
        public new DownstreamRestApiOptions Clone()
        {
            return (CloneInternal() as DownstreamRestApiOptions)!;
        }

        /// <inheritdoc/>
        protected override AuthorizationHeaderProviderOptions CloneInternal()
        {
            return new DownstreamRestApiOptions(this);
        }

        /// <summary>
        /// Scopes required to call the downstream web API.
        /// For instance "user.read mail.read".
        /// For Microsoft identity, in the case of application tokens (token 
        /// requested by the app on behalf of itself), there should be only one scope, and it
        /// should end in "./default")
        /// </summary>
        public IEnumerable<string>? Scopes { get; set; }

        /// <summary>
        /// Optional serializer. Will serialize the input to the web API (if any).
        /// By default, when not provided:
        /// <list type="bullet">
        /// <item><description>If the input derives from <c>HttpInput</c>, it's used as is</description></item>
        /// <item><description>If the input is a string it's used as is an considered a media type json.</description></item>
        /// <item><description>Otherwise, the object is serialized in JSON, with a UTF8 encoding, and a media
        /// type of application/json:
        /// <code>
        /// new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json")
        /// </code>
        /// </description></item>
        /// </list>
        /// </summary>
        public Func<object?, HttpContent?>? Serializer { get; set; }

        /// <summary>
        /// Optional de-serializer. Will de-serialize the output from the web API (if any).
        /// When not provided, the following is returned:
        /// <code>JsonSerializer.Deserialize&lt;TOutput&gt;(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });</code>
        /// </summary>
        public Func<HttpContent?, object?>? Deserializer { get; set; }
    }
}
