﻿using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Command.Files.DownloadFile;
using BetterCms.Module.MediaManager.Command.Files.GetFiles;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Handles site settings logic for Media module Files tab.
    /// </summary>
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class FilesController : CmsControllerBase
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets the files list.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        /// List of files.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult GetFilesList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }

            var model = GetCommand<GetFilesCommand>().ExecuteCommand(options);
            if (model == null)
            {
                success = false;
            }

            return Json(new WireJson { Success = success, Data = model });
        }

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="id">The file id.</param>
        /// <param name="version">The file entity version.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult FileDelete(string id, string version)
        {
            var request = new DeleteMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            if (GetCommand<DeleteMediaCommand>().ExecuteCommand(request))
            {
                Messages.AddSuccess(MediaGlobalization.DeleteFile_DeletedSuccessfully_Message);
                return Json(new WireJson
                {
                    Success = true
                });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Gets files list to insert to content.
        /// </summary>
        /// <returns>
        /// The view.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult FileInsert()
        {
            var files = GetCommand<GetFilesCommand>().ExecuteCommand(new MediaManagerViewModel());
            var success = files != null;
            var view = RenderView("FileInsert", new MediaImageViewModel());

            return ComboWireJson(success, view, files, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Downloads the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>File to download.</returns>
        public ActionResult Download(string id)
        {
            var model = GetCommand<DownloadFileCommand>().ExecuteCommand(id.ToGuidOrDefault());
            if (model != null)
            {
                model.FileStream.Position = 0;
                return File(model.FileStream, model.ContentMimeType, model.FileDownloadName);
            }

            if (!string.IsNullOrWhiteSpace(CmsConfiguration.PageNotFoundUrl))
            {
                return Redirect(HttpUtility.UrlDecode(CmsConfiguration.PageNotFoundUrl));
            }

            return new HttpStatusCodeResult(404);
        }
    }
}
