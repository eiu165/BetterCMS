﻿using System;

using MvcContrib.Sorting;
using MvcContrib.UI.Grid;

namespace BetterCms.Module.Root.Mvc.Grids.GridOptions
{
    [Serializable]
    public class GridOptions : GridSortOptions
    {
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Sets the default sorting options, if values are not set.
        /// </summary>
        public void SetDefaultSortingOptions(string sortColumn, bool isDescending = false)
        {
            if (string.IsNullOrWhiteSpace(Column))
            {
                Column = sortColumn;
                Direction = (isDescending) ? SortDirection.Descending : SortDirection.Ascending;
            }
        }
    }
}