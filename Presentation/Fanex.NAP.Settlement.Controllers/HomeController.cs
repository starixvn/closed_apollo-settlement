// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.29">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// No Permission on page.
        /// </summary>
        /// <returns>Return View result.</returns>
        [HttpGet]
        public ActionResult NonePermission()
        {
            return this.View("~/Areas/UserLogin/Views/Login/NonePermission.cshtml");
        }

        /// <summary>
        /// Redirect first default page.
        /// </summary>
        /// <returns>Return View result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var session = new CurrentSession();
            var returnUrl = Url.Content("~/UserLogin/Login");
            if (session.UserId > 0
                && session.MenuItemList != null && session.MenuItemList.Count > 0)
            {
                var firstMenu = session.MenuItemList.FirstOrDefault();

                if (firstMenu != null)
                {
                    var childMenu = session.MenuItemList.FirstOrDefault(i => i.Parent != null && i.Parent.Id == firstMenu.Id);
                    returnUrl = childMenu != null ? this.Url.Content("~/" + childMenu.Url) : this.Url.Content("~/" + firstMenu.Url);
                }
            }
            else if (session.UserId > 0 && session.MenuItemList != null && session.MenuItemList.Count == 0)
            {
                return this.NonePermission();
            }

            return this.Redirect(returnUrl);
        }
    }
}