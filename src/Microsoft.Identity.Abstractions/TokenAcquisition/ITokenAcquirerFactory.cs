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
        /// <param name="applicationIdentityOptions">Application configuration. Can be an
        /// <see cref="MicrosoftIdentityApplicationOptions"/>.</param>
        /// <returns>An instance of <see cref="ITokenAcquirer"/> that will enable token acquisition.</returns>
        ITokenAcquirer GetTokenAcquirer(IdentityApplicationOptions applicationIdentityOptions);

        /// <summary>
        /// Get a token acquirer for a specific <see cref="MicrosoftIdentityApplicationOptions"/> named configuration
        /// (for instance an ASP.NET Core authentication scheme).
        /// </summary>
        /// <param name="optionName">Name of the Application configuration as defined by the configuration.
        /// For instance in ASP.NET Core it would be the authentication scheme.</param>
        /// <returns>An instance of <see cref="ITokenAcquirer"/> that will enable token acquisition.</returns>
        ITokenAcquirer GetTokenAcquirer(string optionName = "");
    }
}
