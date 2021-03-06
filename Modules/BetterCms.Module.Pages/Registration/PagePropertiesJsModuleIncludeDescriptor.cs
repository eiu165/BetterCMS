﻿using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.properties.js module descriptor.
    /// </summary>
    public class PagePropertiesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagePropertiesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagePropertiesJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.pages.properties")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "loadEditPropertiesDialogUrl", c => c.EditPageProperties("{0}"))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "editPagePropertiesModalTitle", () => PagesGlobalization.EditPageProperties_Title),
                };
        }
    }
}