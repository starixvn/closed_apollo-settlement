// --------------------------------------------------------------------------
// <copyright file="SSOController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
//   <change who="Kaden.Trinh" date="2013.8.15">Check StyleCop rule</change>
// </history>

namespace Fanex.NAP.Settlement.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// SSOController class.
    /// </summary>
    public class SSOController : BaseController
    {
        #region members

        private const int TimeExipration = 10;
        private readonly IFunctionService _functionService;
        private IUserService _userService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;

        #endregion members

        #region ctors

        public SSOController()
            : this(null, null, null, null)
        {
        }

        public SSOController(
            IUserService userService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
             IFunctionService functionService)
        {
            _userService = userService ?? new UserService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
            _functionService = functionService ?? new FunctionService();
        }

        #endregion ctors

        /// <summary>
        /// Action Result Index.
        /// </summary>
        /// <param name="userId">Index user ID.</param>
        /// <param name="onuserId">Index on user ID.</param>
        /// <param name="time">Index time.</param>
        /// <param name="redirectUrl">Redirect Url.</param>
        /// <returns>Redirect URL.</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string userId, string onuserId, string time, Uri redirectUrl = null)
        {
            string currentTime = this._userService.UserSSOGetTime();
            string forwardLink = redirectUrl == null ? FormsAuthentication.DefaultUrl : redirectUrl.ToString();
            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserId = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            var expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= TimeExipration)
            {
                var criteria = new UserSSOCheckInUserCriteria
                {
                    UserId = Convert.ToInt32(userId),
                    SiteId = SiteId,
                    CheckOnUserId = Convert.ToInt32(onuserId),
                    OnUserId = 0,
                    IP = CurrentContext.IP,
                    NetSessionId = Session.SessionID,
                    LoginDate = DateTime.Now,
                    BrowserName = CurrentContext.BrowserName,
                    CountryCode = CurrentContext.CountryCode,
                    Domain = CurrentContext.DomainName,
                    Platform = CurrentContext.Platform
                };
                this._userService.UserSSOCheckIn(ref criteria);
                if (criteria.OnUserId != 0)
                {
                    var user = this._userService.GetById(criteria.UserId) as User;
                    this._currentSession.UserId = user.Id;
                    this._currentSession.Username = user.Username;
                    this._currentSession.OnUserId = criteria.OnUserId;
                    this._currentSession.SessionId = Session.SessionID.ToString();
                    this._currentSession.Level = this._userService.GetUserSiteLevel(user.Id, this.SiteId);

                    if (this.SiteId != 0)
                    {
                        this._currentSession.MenuItemList = this._menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                user.Id,
                                                                                SiteId,
                                                                                this.Environment,
                                                                                true) as IList<MenuItem>;
                    }

                    this.CreateAuthInfo(user);
                    return this.Redirect(forwardLink);
                }
            }

            return this.Redirect(FormsAuthentication.LoginUrl);
        }

        #region utilities

        /// <summary>
        /// Create authentication information in cookies.
        /// </summary>
        /// <param name="user">User object.</param>
        private void CreateAuthInfo(User user)
        {
            var currentDomain = CurrentContext.GetCurrentDomain();

            var authInfo = new AuthInfo();

            authInfo.UserId = user.Id;
            authInfo.OnUserId = this._currentSession.OnUserId;

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
        /// Set new value to user criteria.
        /// </summary>
        /// <param name="userId">User id parameter.</param>
        /// <param name="onuserId">On User id.</param>
        /// <returns>Return UserCriteria.</returns>
        private UserSSOCheckInUserCriteria SetUserCriteria(int userId, int onuserId)
        {
            return new UserSSOCheckInUserCriteria
            {
                UserId = userId,
                SiteId = SiteId,
                CheckOnUserId = onuserId,
                OnUserId = 0,
                IP = CurrentContext.IP,
                NetSessionId = Session.SessionID,
                LoginDate = DateTime.Now,
                BrowserName = CurrentContext.BrowserName,
                CountryCode = CurrentContext.CountryCode,
                Domain = CurrentContext.DomainName,
                Platform = CurrentContext.Platform
            };
        }

        #endregion utilities
    }
}