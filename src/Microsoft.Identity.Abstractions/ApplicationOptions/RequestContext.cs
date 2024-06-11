// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Holds information about an incoming request.
    /// </summary>
    public class RequestContext
    {
        private Dictionary<string, object>? _propertyBag;

        /// <summary>
        /// Gets an <see cref= "IDictionary{String, Object}" /> that enables custom extensibility scenarios.
        /// </summary>
        public IDictionary<string, object> PropertyBag
        {
            get
            {
                if (_propertyBag == null)
                    Interlocked.CompareExchange(ref _propertyBag, new Dictionary<string, object>(StringComparer.Ordinal), null);
                
                return _propertyBag;
            }
        }
    }
}
