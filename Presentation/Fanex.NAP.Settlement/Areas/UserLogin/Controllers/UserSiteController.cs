// --------------------------------------------------------------------------
// <copyright file="UserSiteController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Settlement.Areas.UserLogin.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// UserSiteController class.
    /// </summary>
    public class UserSiteController : BaseController
    {
        #region members

        private ISiteService _siteService;
        private ICurrentSession _currentSession;
        private IUserService _userService;

        #endregion members

        #region ctors

        public UserSiteController()
            : this(null, null, null)
        {
        }

        public UserSiteController(ISiteService siteService, ICurrentSession currentSession, IUserService userService)
        {
            _siteService = _siteService ?? new SiteService();
            _currentSession = _currentSession ?? new CurrentSession();
            _userService = _userService ?? new UserService();
        }

        #endregion ctors

        #region actions

        /// <summary>
        /// Site Action.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult Site()
        {
            if (HttpContext.User.Identity.IsAuthenticated && _currentSession.UserId > 0)
            {
                var siteUrlList = _siteService.GetSiteUrlByUser(_currentSession.UserId, this.Environment);

                // for re-checkin
                ViewBag.OnUserId = _currentSession.OnUserId;

                return View(siteUrlList);
            }

            return RedirectToAction("logout", "login");
        }

        /// <summary>
        /// Forward action.
        /// </summary>
        /// <param name="id">Site Id value.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult Forward(int id)
        {
            var siteUrl = _siteService.GetSiteUrlById(id);

            if (siteUrl == null)
            {
                return HttpNotFound();
            }

            string time = _userService.UserSSOGetTime();
            var url = "{0}/userlogin/sso?userid={1}&onuserid={2}&time={3}";
            var userId = Cryptography.CfsEnCrypt(_currentSession.UserId.ToString());
            var onuserId = Cryptography.CfsEnCrypt(_currentSession.OnUserId.ToString());
            return new RedirectResult(string.Format(url, siteUrl.Url.Trim(), userId, onuserId, Cryptography.CfsEnCrypt(time)));
        }

        #endregion actions
    }
}