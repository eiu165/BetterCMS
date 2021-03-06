﻿using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Pages.Api.DataContracts;
using BetterCms.Module.Pages.Models;
using BetterCms.Sandbox.Mvc4.Models;

namespace BetterCms.Sandbox.Mvc4.Controllers
{
    public class SandboxController : Controller
    {
        public ActionResult Content()
        {
            return Content("Hello from the web project controller.");
        }

        public ActionResult Hello()
        {
            return PartialView("Partial/Hello");
        }

        public ActionResult Widget05()
        {
            return PartialView("~/Views/Widgets/05.cshtml");
        }

        [AllowAnonymous]
        public ActionResult Login(string roles)
        {
            //            var roles = string.Join(",", Roles.GetRolesForUser(string.Empty));
            if (string.IsNullOrEmpty(roles))
            {
                roles = "Owner";
            }

            var authTicket = new FormsAuthenticationTicket(1, "Better CMS test user", DateTime.Now, DateTime.Now.AddMonths(1), true, roles);

            var cookieContents = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
            {
                Expires = authTicket.Expiration,
                Path = FormsAuthentication.FormsCookiePath
            };

            HttpContext.Response.Cookies.Add(cookie);

            return Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();

            return Redirect("/");
        }

        public ActionResult TestApi()
        {
            /*PagesApiContext.Events.PageCreated += EventsOnPageCreated;

            PagesApiContext.Events.OnPageCreated(new PageProperties());

            ApiContext.Events.HostStart += Core_HostStart;*/

            var message = string.Empty;

            DataListResponse<BlogPost> results;
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                using (var papi = CmsContext.CreateApiContextOf<PagesApiContext>())
                {
                    var aRequest = new GetContentHistoryRequest(new Guid("AE04E233-4E88-4A9F-87BC-A1CC00F2C173"));
                    var aresults = papi.GetContentHistory(aRequest);

                    if (aresults.Items.Count > 0)
                    {
                        message = string.Format("{0}<br />Total count:{2},  Item titles: {1}", message, string.Join("; ", aresults.Items.Select(t => t.Name)), aresults.TotalCount);
                    }

                    aRequest = new GetContentHistoryRequest(new Guid("AE04E233-4E88-4A9F-87BC-A1CC00F2C173"), a => a.Name.ToLower().Contains("poop"), o => o.Name, orderDescending: true);
                    aresults = papi.GetContentHistory(aRequest);

                    if (aresults.Items.Count > 0)
                    {
                        message = string.Format("{0}<br />Total count:{2},  Item titles: {1}", message, string.Join("; ", aresults.Items.Select(t => t.Name)), aresults.TotalCount);
                    }

                    /*var request = new GetDataRequest<Layout>(3, 2, orderDescending:true, order:t =>t.Name);
                    results = pagesApi.GetLayouts(request);*/

                    /*var request = new GetDataRequest<LayoutRegion>(orderDescending: true, order: t => t.Region.RegionIdentifier);
                    request.AddPaging(2, 2);*/

                    /*var request = new GetBlogPostsRequest(b => b.ExpirationDate.HasValue, includePrivate: true, includeUnpublished: true, itemsCount: 3);
                    results = api.GetBlogPosts(request);

                    if (results.Items.Count > 0)
                    {
                        message = string.Format(
                            "{0}<br />Total count:{2},  Item titles: {1}", message, string.Join("; ", results.Items.Select(t => t.Title)), results.TotalCount);
                    }

                    request = new GetBlogPostsRequest(order: b => b.Title, orderDescending: true, includePrivate: true, includeUnpublished: true);
                    request.AddPaging(3, 3);
                    results = api.GetBlogPosts(request);

                    if (results.Items.Count > 0)
                    {
                        message = string.Format(
                            "{0}<br />Total count:{2}, Item titles: {1}", message, string.Join("; ", results.Items.Select(t => t.Title)), results.TotalCount);
                    }

                    request = new GetBlogPostsRequest(
                        order: b => b.Title, orderDescending: true, itemsCount: 5, startItemNumber: 3, includeUnpublished: true, includePrivate: true);
                    results = api.GetBlogPosts(request);*/
                }
            }

            return Content(message);
        }

        void Core_HostStart(SingleItemEventArgs<HttpApplication> args)
        {
            throw new NotImplementedException();
        }

        private void EventsOnPageCreated(SingleItemEventArgs<PageProperties> args)
        {

        }

        public ActionResult TestNavigationApi()
        {

            var message = new StringBuilder("No sitemap data found!");

            return Content(message.ToString());
        }

        [AllowAnonymous]
        public ActionResult LoginJson(LoginViewModel login)
        {
            Login(string.Empty);

            return Json(new { Success = true });
        }

        [AllowAnonymous]
        public ActionResult LogoutJson(LoginViewModel login)
        {
            Logout();

            return Json(new { Success = true });
        }

        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}