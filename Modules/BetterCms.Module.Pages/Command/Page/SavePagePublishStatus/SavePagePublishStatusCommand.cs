﻿using System;

using BetterCms.Api;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.SavePagePublishStatus
{
    /// <summary>
    /// Command to updated page status.
    /// </summary>
    public class SavePagePublishStatusCommand : CommandBase, ICommand<SavePagePublishStatusRequest, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if succeeded, otherwise <c>false</c></returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">If page status is not correct.</exception>
        public bool Execute(SavePagePublishStatusRequest request)
        {
            DemandAccess(RootModuleConstants.UserRoles.PublishContent);     // This would rise security exception if user has no access.

            var page = UnitOfWork.Session
                .QueryOver<PageProperties>().Where(p => p.Id == request.PageId && !p.IsDeleted)
                .SingleOrDefault<PageProperties>();

            if (page != null)
            {
                var initialStatus = page.Status;

                if (page.Status == PageStatus.Draft || page.Status == PageStatus.Preview)
                {
                    var message = string.Format(PagesGlobalization.SavePageStatus_PageIsInInappropriateStatus_Message);
                    var logMessage = string.Format("Draft/Preview page id={0} can not be published.", page.Id);
                    throw new ValidationException(() => message, logMessage);
                }

                if (request.IsPublished)
                {
                    page.Status = PageStatus.Published;
                    page.PublishedOn = DateTime.Now;
                }
                else
                {
                    page.Status = PageStatus.Unpublished;
                }

                Repository.Save(page);                
                UnitOfWork.Commit();

                if (page.Status != initialStatus)
                {
                    PagesApiContext.Events.OnPagePublishStatusChanged(page);
                }
            }

            return true;
        }
    }
}