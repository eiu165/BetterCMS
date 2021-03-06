﻿using System.Security.Principal;

namespace BetterCms.Core.Services
{
    /// <summary>
    /// Security service contract.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Gets the name of the get current principal.
        /// </summary>
        /// <value>
        /// The name of the get current principal.
        /// </value>
        string CurrentPrincipalName { get; }

        /// <summary>
        /// Gets the current principal.
        /// </summary>
        /// <returns>Current IPrincipal.</returns>
        IPrincipal GetCurrentPrincipal();

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>Role list.</returns>
        string[] GetAllRoles();

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>Better CMS security configuration.</returns>
        ICmsSecurityConfiguration GetConfiguration();

        /// <summary>
        /// Determines whether the specified principal is authorized.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        ///   <c>true</c> if the specified principal is authorized; otherwise, <c>false</c>.
        /// </returns>
        bool IsAuthorized(IPrincipal principal, string roles);

        /// <summary>
        /// Determines whether the current principal is authorized.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>
        ///   <c>true</c> if the current principal is authorized; otherwise, <c>false</c>.
        /// </returns>
        bool IsAuthorized(string roles);
    }
}
