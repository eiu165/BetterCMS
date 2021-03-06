﻿using System;
using System.Linq;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Root.Api.Events;
using BetterCms.Module.Root.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public class RootApiContext : DataApiContext
    {
        private static readonly RootApiEvents events;

        /// <summary>
        /// Initializes the <see cref="RootApiContext" /> class.
        /// </summary>
        static RootApiContext()
        {
            events = new RootApiEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        public RootApiContext(ILifetimeScope lifetimeScope, IRepository repository = null)
            : base(lifetimeScope, repository)
        {
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public new static RootApiEvents Events
        {
            get
            {
                return events;
            }
        }

        public bool IsContentMigrated(string moduleName, long contentVersion)
        {
            try
            {
                return Repository.AsQueryable<ModuleContentVersion>(v => v.ModuleName == moduleName && v.ContentVersion == contentVersion).Any();
            }
            catch (Exception ex)
            {
                const string message = "Failed to perform version check.";
                Logger.Error(message, ex);
                throw new CmsApiException(message, ex);
            }
        }

        public void ContentMigrated(string moduleName, long contentVersion)
        {
            try
            {
                var version = new ModuleContentVersion { ModuleName = moduleName, ContentVersion = contentVersion };
                Repository.Save(version);

                UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                const string message = "Failed to perform version saving.";
                Logger.Error(message, ex);
                throw new CmsApiException(message, ex);
            }
        }

    }
}