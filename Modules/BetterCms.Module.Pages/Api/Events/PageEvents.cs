﻿using BetterCms.Api;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Pages.Api.Events
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class PagesApiEvents : EventsBase
    {        
        /// <summary>
        /// Occurs when page is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PageCreated;
        
        /// <summary>
        /// Occurs when page is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PageDeleted;
        
        /// <summary>
        /// Occurs when a page cloned.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PageCloned;

        /// <summary>
        /// Occurs when a page properties is changed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PagePropertiesChanged;
 
        /// <summary>
        /// Occurs when a page publish status is changed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PagePublishStatusChanged;

        /// <summary>
        /// Occurs when a page SEO status is changed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PageSeoStatusChanged;

        /// <summary>
        /// Occurs when a widget is inserted.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<PageContent>> PageContentInserted;
         
        /// <summary>
        /// Called when page is created.
        /// </summary>
        public void OnPageCreated(PageProperties page)
        {
            if (PageCreated != null)
            {
                PageCreated(new SingleItemEventArgs<PageProperties>(page));
            }
        }

        /// <summary>
        /// Called when page is deleted.
        /// </summary>
        public void OnPageDeleted(PageProperties page)
        {
            if (PageDeleted != null)
            {
                PageDeleted(new SingleItemEventArgs<PageProperties>(page));
            }
        }

        public void OnPageCloned(PageProperties page)
        {
            if (PageCloned != null)
            {
                PageCloned(new SingleItemEventArgs<PageProperties>(page));                
            }
        }

        public void OnPagePropertiesChanged(PageProperties page)
        {
            if (PagePropertiesChanged != null)
            {
                PagePropertiesChanged(new SingleItemEventArgs<PageProperties>(page));
            }
        }

        public void OnPagePublishStatusChanged(PageProperties page)
        {
            if (PagePublishStatusChanged != null)
            {
                PagePublishStatusChanged(new SingleItemEventArgs<PageProperties>(page));
            }
        }

        public void OnPageSeoStatusChanged(PageProperties page)
        {
            if (PageSeoStatusChanged != null)
            {
                PageSeoStatusChanged(new SingleItemEventArgs<PageProperties>(page));
            }
        }
        
        public void OnPageContentInserted(PageContent pageContent)
        {
            if (PageContentInserted != null)
            {
                PageContentInserted(new SingleItemEventArgs<PageContent>(pageContent));
            }
        }
    }
}
