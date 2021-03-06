﻿using System;

namespace BetterCms.Module.Root.Mvc.Grids.GridOptions
{
    [Serializable]
    public class SearchableGridOptions : GridOptions
    {
        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>
        /// The search query.
        /// </value>
        public string SearchQuery { get; set; }
    }
}