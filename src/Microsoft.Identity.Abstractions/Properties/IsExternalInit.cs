// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NETFRAMEWORK || NETSTANDARD

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Polyfill that enables <c>init</c>-only setters on target frameworks that do not
    /// ship the type in their reference assemblies.
    /// </summary>
    internal static class IsExternalInit
    {
    }
}

#endif
