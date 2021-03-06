﻿using System;
using System.Collections.Generic;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    public interface ITagService
    {
        /// <summary>
        /// Saves the page tags.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newCreatedTags">A list of new tags.</param>
        void SavePageTags(PageProperties page, IList<string> tags, out IList<Tag> newCreatedTags);

        /// <summary>
        /// Gets the page tag names.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>List fo tag names</returns>
        IList<string> GetPageTagNames(Guid pageId);
    }
}