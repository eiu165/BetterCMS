﻿using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Command.History.GetContentHistory;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.seo.js module descriptor.
    /// </summary>
    public class HistoryJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeoJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public HistoryJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.history", "/file/bcms-pages/scripts/bcms.pages.history")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadContentHistoryDialogUrl", controller => controller.ContentHistory("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadContentVersionPreviewUrl", controller => controller.ContentVersion("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "restoreContentVersionUrl", controller => controller.RestorePageContentVersion("{0}")),
                };

            Globalization = new IActionProjection[]
                {
                     new JavaScriptModuleGlobalization(this, "contentHistoryDialogTitle", () => PagesGlobalization.ContentHistory_DialogTitle),
                     new JavaScriptModuleGlobalization(this, "contentVersionRestoryConfirmation", () => PagesGlobalization.ContentHistory_Restore_ConfirmationMessage)
                };
        }
    }
}