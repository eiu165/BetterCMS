﻿using System;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;

namespace BetterCms.Module.Root.Mvc
{
    /// <summary>
    /// A base class for commands.
    /// </summary>
    public abstract class CommandBase : ICommandBase
    {
        /// <summary>
        /// Gets or sets a command context.
        /// </summary>
        /// <value>
        /// A command executing context.
        /// </value>
        public ICommandContext Context { get; set; }

        /// <summary>
        /// Gets or sets the repository. This property is auto wired.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public virtual IRepository Repository { get; set; }

        /// <summary>
        /// Gets or sets the unit of work. This property is auto wired.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        public virtual IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Gets or sets the security service.
        /// </summary>
        /// <value>
        /// The security service.
        /// </value>
        public virtual ISecurityService SecurityService { get; set; }

        /// <summary>
        /// Demands the access.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <exception cref="SecurityException">Forbidden: Access is denied.</exception>
        protected void DemandAccess(params string[] roles)
        {
            if (SecurityService == null)
            {
                throw new ArgumentNullException();
            }

            if (!SecurityService.IsAuthorized(Context.User, string.Join(",", roles)))
            {
                throw new SecurityException("Forbidden: Access is denied.");
            }
        }
    }
}