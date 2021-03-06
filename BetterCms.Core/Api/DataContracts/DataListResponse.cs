﻿using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Core.Api.DataContracts
{
    public class DataListResponse<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataListResponse{TEntity}" /> class.
        /// </summary>
        public DataListResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataListResponse{TEntity}" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="totalCount">The total count.</param>
        public DataListResponse(IList<TEntity> items = null, int totalCount = 0)
        {
            Items = items;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Gets or sets the list of items.
        /// </summary>
        /// <value>
        /// The list of items.
        /// </value>
        public IList<TEntity> Items { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }
    }
}
