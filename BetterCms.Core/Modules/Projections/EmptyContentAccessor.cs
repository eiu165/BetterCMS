﻿using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Core.Modules.Projections
{
    public class EmptyContentAccessor : ContentAccessor<IContent>
    {
        private readonly string contentHtml;

        public EmptyContentAccessor(string contentHtml)
            : base(null, null)
        {            
            this.contentHtml = contentHtml;
        }

        public override string GetContentWrapperType()
        {
            return null;
        }
        
        public override string GetTitle()
        {
            return null;
        }

        public override string GetHtml(HtmlHelper html)
        {
            return string.Format(contentHtml);
        }

        public override string GetCustomStyles(HtmlHelper html)
        {
            return null;
        }

        public override string GetCustomJavaScript(HtmlHelper html)
        {
            return null;
        }
    }
}
