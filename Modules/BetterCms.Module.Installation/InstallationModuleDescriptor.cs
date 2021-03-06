﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Installation
{
    /// <summary>
    /// Templates module descriptor.
    /// </summary>
    public class InstallationModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "installation";

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public InstallationModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of pages module.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "Templates module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public override int Order
        {
            get
            {
                return int.MaxValue - 100;
            }
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>
        /// <returns>
        /// Enumerator of known module style sheet files.
        /// </returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[] { new CssIncludeDescriptor(this, "bcms.installation.css", "bcms.installation.min.css", true) };
        }
    }
}
