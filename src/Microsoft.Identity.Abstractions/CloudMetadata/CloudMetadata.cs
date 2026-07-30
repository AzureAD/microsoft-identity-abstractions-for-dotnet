// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// An immutable, case-insensitive bag of cloud-specific metadata for a single Azure cloud, keyed by
    /// the well-known literals in <see cref="AbstractionsCloudKeys"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the neutral data-transfer type used to move cloud-specific values (for example the FIC
    /// token-exchange audience) between SDKs without any of them depending on another's concrete types.
    /// It holds only string values addressed by string keys, so new keys can be added without changing
    /// this type or breaking existing consumers.
    /// </para>
    /// <para>
    /// It contains <b>no built-in values</b>: the public list of clouds is owned by MSAL and the
    /// internal-only list is owned by MISE. This package supplies only the contract shape.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var metadata = new CloudMetadata(new Dictionary&lt;string, string&gt;
    /// {
    ///     [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchangeUSGov",
    /// });
    ///
    /// if (metadata.TryGetValue(AbstractionsCloudKeys.TokenExchangeAudience, out string? audience))
    /// {
    ///     // use audience
    /// }
    /// </code>
    /// </example>
    public sealed class CloudMetadata
    {
        private readonly Dictionary<string, string> _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudMetadata"/> class from the supplied key/value
        /// pairs. The values are copied into a case-insensitive dictionary, so the caller's dictionary can
        /// be mutated afterwards without affecting this instance.
        /// </summary>
        /// <param name="values">The cloud-specific key/value pairs. Keys should come from
        /// <see cref="AbstractionsCloudKeys"/> (or an SDK-specific extension of that vocabulary).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        public CloudMetadata(IReadOnlyDictionary<string, string> values)
        {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(values);
#else
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }
#endif

            var copy = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, string> pair in values)
            {
                copy[pair.Key] = pair.Value;
            }

            _values = copy;
        }

        /// <summary>
        /// Gets the case-insensitive collection of cloud-specific key/value pairs held by this instance.
        /// </summary>
        public IReadOnlyDictionary<string, string> Values
        {
            get
            {
                return _values;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The metadata key, typically one of the <see cref="AbstractionsCloudKeys"/> literals.</param>
        /// <param name="value">When this method returns, contains the value for <paramref name="key"/> if it
        /// was found; otherwise <c>null</c>.</param>
        /// <returns><c>true</c> if the key was present; otherwise <c>false</c>.</returns>
        public bool TryGetValue(string key, out string? value)
        {
            return _values.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the value associated with the specified key, or <c>null</c> if the key is not present.
        /// </summary>
        /// <param name="key">The metadata key, typically one of the <see cref="AbstractionsCloudKeys"/> literals.</param>
        /// <returns>The value, or <c>null</c> when the key is absent.</returns>
        public string? GetValueOrDefault(string key)
        {
            return _values.TryGetValue(key, out string? value) ? value : null;
        }
    }
}
