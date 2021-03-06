﻿using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.MediaManager.ViewModels;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Blog.Commands.GetAuthorList
{
    public class GetAuthorListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<AuthorViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with blog post view models</returns>
        public SearchableGridViewModel<AuthorViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<AuthorViewModel> model;

            request.SetDefaultSortingOptions("Name");

            var query = Repository
                .AsQueryable<Author>();

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(a => a.Name.Contains(request.SearchQuery));
            }

            var authors = query
                .Select(author =>
                    new AuthorViewModel
                        {
                            Id = author.Id,
                            Version = author.Version,
                            Name = author.Name,
                            Image = author.Image == null ? null :
                                    new ImageSelectorViewModel
                                        {
                                            ImageId = author.Image.Id,
                                            ImageVersion = author.Image.Version,
                                            ImageUrl = author.Image.PublicUrl,
                                            ThumbnailUrl = author.Image.PublicThumbnailUrl,
                                            ImageTooltip = author.Image.Caption
                                        }
                        });

            var count = query.ToRowCountFutureValue();
            authors = authors.AddSortingAndPaging(request);

            model = new SearchableGridViewModel<AuthorViewModel>(authors.ToList(), request, count.Value);

            return model;
        }
    }
}