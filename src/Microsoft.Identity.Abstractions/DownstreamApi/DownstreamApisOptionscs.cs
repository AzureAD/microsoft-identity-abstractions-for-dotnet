// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options for a set of Downstream APIs. You would usually
    /// use them when you want to configure a set of downstream APIs
    /// programmatically (vs using configuration).
    /// </summary>
    public class DownstreamApisOptions : Dictionary<string, DownstreamApiOptions>
    {
    }
}
