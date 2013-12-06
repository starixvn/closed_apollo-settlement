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
            var session = new Core.Web.Session.CurrentSession();
            var returnUrl = Url.Content("~/UserLogin/Login");
            if (session.UserId > 0
                && session.Menu != null && session.Menu.Count > 0)
            {
                var firstMenu = session.Menu.FirstOrDefault();

                if (firstMenu != null)
                {
                    var childMenu = session.Menu.FirstOrDefault(i => i.Parent != null && i.Parent.Id == firstMenu.Id);
                    returnUrl = childMenu != null ? this.Url.Content("~/" + childMenu.Url) : this.Url.Content("~/" + firstMenu.Url);
                }
            }
            else if (session.UserId > 0 && session.Menu != null && session.Menu.Count == 0)
            {
                return this.NonePermission();
            }

            return this.Redirect(returnUrl);
        }
    }
}