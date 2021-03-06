﻿using BetterCms.Api;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Api.Events
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class PagesApiEvents
    {
        /// <summary>
        /// Occurs when a redirect is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryCreated;

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryUpdated;

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryDeleted;

        public void OnCategoryCreated(Category category)
        {
            if (CategoryCreated != null)
            {
                CategoryCreated(new SingleItemEventArgs<Category>(category));
            }
        }

        public void OnCategoryUpdated(Category category)
        {
            if (CategoryUpdated != null)
            {
                CategoryUpdated(new SingleItemEventArgs<Category>(category));
            }
        }

        public void OnCategoryDeleted(Category category)
        {
            if (CategoryDeleted != null)
            {
                CategoryDeleted(new SingleItemEventArgs<Category>(category));
            }        
        }
    }
}
