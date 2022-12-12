// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Token acquirer factory.
    /// </summary>
    public interface ITokenAcquirerFactory
    {
        /// <summary>
        /// Get a token acquirer given a set of application identity options.
        /// </summary>
        /// <param name="identityApplicationOptions"> the <see cref="IdentityApplicationOptions" />
        /// that describe the <see cref="ITokenAcquirer"/> instance to return.
        /// For Microsoft applications use <see cref="MicrosoftIdentityApplicationOptions"/>.</param>
        /// <returns>An instance of <see cref="ITokenAcquirer"/> for acquiring tokens.</returns>
        ITokenAcquirer GetTokenAcquirer(IdentityApplicationOptions identityApplicationOptions);

        /// <summary>
        /// Get a token acquirer for a specific named configuration
        /// (for instance an ASP.NET Core authentication scheme).
        /// </summary>
        /// <param name="optionName">Name of the Application configuration as defined by the configuration.
        /// For instance in ASP.NET Core it could be the authentication scheme.</param>
        /// <returns>An instance of <see cref="ITokenAcquirer"/> for acquiring tokens.</returns>
        ITokenAcquirer GetTokenAcquirer(string optionName = "");
    }
}
