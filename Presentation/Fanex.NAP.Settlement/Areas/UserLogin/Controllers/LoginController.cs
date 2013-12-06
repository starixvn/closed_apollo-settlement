// --------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Settlement.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Logging;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.NAP.Settlement.Web.Configuration;
    using Fanex.NAP.Settlement.Web.Services;

    /// <summary>
    /// LoginController class, contains all action of login, logout.
    /// </summary>
    public class LoginController : BaseController
    {
        #region members

        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly ISiteService _siteService;
        private readonly IMenuItemService _menuItemService;
        private readonly ICurrentSession _currentSession;
        private readonly IFunctionService _functionService;
        private IMaintainanceService _maintenanceService;

        #endregion members

        #region ctors

        public LoginController()
            : this(null, null, null, null, null, null, null)
        {
        }

        public LoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
            IFunctionService functionService,
            IMaintainanceService maintenanceService)
        {
            _userService = userService ?? new UserService();
            _groupService = groupService ?? new GroupService();
            _siteService = siteService ?? new SiteService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
            _functionService = functionService ?? new FunctionService();
            _maintenanceService = maintenanceService ?? new MaintainanceService();
        }

        #endregion ctors

        #region sign in

        /// <summary>
        /// Action Index.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!NapSettings.Settings.IsNapHub && !IsSiteLogin)
            {
                // If login-directly feature is disabled then redirect users to NAP Hub
                return Redirect(NapSettings.Settings.HubUrl);
            }

            if (_maintenanceService.GetDowntimeInfo().IsDowntime)
            {
                return RedirectToAction("Offline", "Common");
            }

            return View();
        }

        /// <summary>
        /// Action Index: Login with username and password.
        /// </summary>
        /// <param name="username">Username string.</param>
        /// <param name="password">Password string.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [ValidateInput(false)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
              "StyleCop.CSharp.Nexcel.NexcelCustomRules",
              "SP2101:MethodMustNotContainMoreLinesThan",
              Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Index(string username, string password)
        {
            if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty())
            {
                var user = _userService.GetByUsername(username.Trim()) as User;
                try
                {
                    if (user != null)
                    {
                        password = Cryptography.CfsEnCode(password);

                        if (password == user.Password)
                        {
                            var group = user.Group == null ? null : _groupService.GetById(user.Group.Id);

                            // Gets all sites of logging-in user
                            var siteLevelList = _siteService.GetSiteLevelByUser(user.Id, Environment);

                            if ((group == null || group.IsActive) && siteLevelList.Any(x => x.IsActive))
                            {
                                // Check-in user
                                var criteria = SetUserCheckInCriteria(user.Id);
                                _userService.UserCheckIn(ref criteria);

                                // Saves user session
                                SaveUserSession(user, username, criteria.OnUserId);

                                var currentFunction = _functionService.GetByUserAndSiteIncludeGroup(user.Id, NapSettings.Settings.SiteId, NapSettings.Settings.Environment);
                                AppSession.CurrentUser = user;
                                AppSession.RoleAccess = PermissionService.GetRoleAccess(currentFunction);

                                // Redirects user
                                if (this.IsSiteLogin)
                                {
                                    if (UserIsActiveOnThisSite(siteLevelList))
                                    {
                                        return Redirect(FormsAuthentication.DefaultUrl);
                                    }
                                    else
                                    {
                                        DoSignOut();
                                        ViewBag.Message = ViewResources.Your_account_was_suspended;
                                    }
                                }
                                else
                                {
                                    return RedirectToRouteResult(siteLevelList);
                                }
                            }
                            else
                            {
                                // user account was suspended on all sites
                                ViewBag.Message = siteLevelList.Any()
                                    ? ViewResources.Your_account_was_suspended
                                    : ViewResources.You_have_no_authorization;
                            }
                        }
                        else
                        {
                            // wrong password
                            ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                        }
                    }
                    else
                    {
                        // user doesn't exist
                        ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    }
                }
                catch (Exception exception)
                {
                    LogService.Error(exception.Message, exception);
                    throw;
                }
            }
            else
            {
                ViewBag.Message = "Invalid input!";
            }

            return View();
        }

        #endregion sign in

        #region sign out

        public ActionResult LogOut()
        {
            DoSignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }

        #endregion sign out

        #region re check in

        /// <summary>
        /// Recheck in action.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ReCheckLogin()
        {
            var maintenanceInfo = _maintenanceService.GetDowntimeInfo();

            if (maintenanceInfo.IsDowntime)
            {
                return Json("-1");
            }

            // Check if not nap hub
            if (this.SiteId != 0)
            {
                var siteStatus = _siteService.GetStatusById(this.SiteId);

                if (!siteStatus)
                {
                    return Json("1");
                }
            }

            var criteria = new UserReCheckInUserCriteria { OnUserId = Convert.ToInt32(_currentSession.OnUserId) };
            _userService.ReUserCheckIn(ref criteria);
            return Json(criteria.IsKickedOut ? "1" : "0");
        }

        #endregion re check in

        #region helpers

        private void CreateUserMenu(User user)
        {
            _currentSession.Menu = (IList<MenuItem>)_menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                    user.Id,
                                                                                    SiteId,
                                                                                    Environment,
                                                                                    true);
        }

        /// <summary>
        /// Create authentication information.
        /// </summary>
        /// <param name="user">User object.</param>
        private void CreateAuthInfo(User user)
        {
            var currentDomain = CurrentContext.GetCurrentDomain();

            var authInfo = new AuthInfo { UserId = user.Id, OnUserId = this._currentSession.OnUserId };

            var authTicket = new FormsAuthenticationTicket(
                 1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// Redirect To Route result.
        /// </summary>
        /// <param name="siteLevelList">Site level.</param>
        /// <returns>Return route result.</returns>
        private RedirectToRouteResult RedirectToRouteResult(IEnumerable<SiteLevel> siteLevelList)
        {
            // user has access to multiple sites
            if (siteLevelList.Where(s => s.IsActive).Count() > 1)
            {
                return RedirectToAction("Site", "UserSite");
            }

            var site = siteLevelList.Where(s => s.IsActive).First().Site;

            var listSiteUrl = _siteService.GetSiteUrlBySiteId(site.Id, this.Environment).Where(u => u.IsActive).ToList();

            // user has acces to one site
            if (listSiteUrl.Count() > 1)
            {
                return RedirectToAction("Site", "UserSite");
            }

            return RedirectToAction("Forward", "UserSite", new { id = listSiteUrl.First().SiteUrlId });
        }

        /// <summary>
        /// Checks if logging-in user is active on this site.
        /// </summary>
        /// <param name="siteLevelList">Site level.</param>
        /// <returns>Return boolean.</returns>
        private bool UserIsActiveOnThisSite(IEnumerable<SiteLevel> siteLevelList)
        {
            bool userIsActiveOnThisSite = false;

            foreach (var siteLevel in siteLevelList)
            {
                if (siteLevel.Site.Id == this.SiteId && siteLevel.IsActive)
                {
                    userIsActiveOnThisSite = true;
                }
            }

            return userIsActiveOnThisSite;
        }

        /// <summary>
        /// Set new value to user criteria.
        /// </summary>
        /// <param name="userId">User id parameter.</param>
        /// <returns>Return user criteria.</returns>
        private UserCheckInUserCriteria SetUserCheckInCriteria(int userId)
        {
            return new UserCheckInUserCriteria
            {
                UserId = userId,
                SiteId = this.SiteId,
                NetSessionId = Session.SessionID,
                Ip = CurrentContext.IP,
                LoginDate = DateTime.Now,
                BrowserName = CurrentContext.BrowserName,
                CountryCode = CurrentContext.CountryCode,
                Domain = CurrentContext.DomainName,
                Platform = CurrentContext.Platform
            };
        }

        /// <summary>
        /// Save user session.
        /// </summary>
        /// <param name="user">User parameter.</param>
        /// <param name="userName">User name.</param>
        /// <param name="userId">On user id.</param>
        private void SaveUserSession(User user, string userName, int userId)
        {
            _currentSession.Level = _userService.GetUserSiteLevel(user.Id, this.SiteId);
            _currentSession.UserId = user.Id;
            _currentSession.Username = userName;
            _currentSession.OnUserId = userId;
            _currentSession.SessionId = Session.SessionID.ToString();

            // current site is not NAP Hub
            if (this.SiteId != 0)
            {
                CreateUserMenu(user);
            }

            CreateAuthInfo(user);
        }

        private void DoSignOut()
        {
            _userService.UserLogOut(new LogOutUserCriteria
            {
                OnUserId = _currentSession.OnUserId
            });
            FormsAuthentication.SignOut();
            _currentSession.ClearAll();
        }

        #endregion helpers
    }
}