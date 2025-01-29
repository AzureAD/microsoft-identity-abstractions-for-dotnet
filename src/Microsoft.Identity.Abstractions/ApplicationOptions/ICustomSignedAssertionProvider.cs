// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Interface to implement loading of a custom signed assertion providers.
    /// </summary>
    public interface ICustomSignedAssertionProvider : ICredentialSourceLoader
    {
        /// <summary>
        /// Configuration friendly name of the custom signed assertion provider as it
        /// will appear in the <see cref="CredentialDescription.CustomSignedAssertionProviderName"/>
        /// Can be <c>null</c> in which case developers will need to pass-in the fully
        /// qualified name of the implementing class.
        /// </summary>
        abstract string Name { get; }
    }
}
