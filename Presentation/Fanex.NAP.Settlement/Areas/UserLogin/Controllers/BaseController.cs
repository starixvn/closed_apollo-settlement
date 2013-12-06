// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Settlement.Areas.UserLogin.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Context;

    /// <summary>
    /// Base Controller.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly string Environment = NapSettings.Settings.Environment;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        protected readonly CurrentContext CurrentContext;

        private bool _issiteLogin = NapSettings.Settings.EnableDirectLogin;

        private ViewFlags _viewFlags;

        public BaseController()
        {
            CurrentContext = new CurrentContext();

            ViewFlags = new ViewFlags
            {
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };
        }

        /// <summary>
        /// Gets a value indicating whether.
        /// </summary>
        /// <value>If "0", users must login through NAP Hub.</value>
        public bool IsSiteLogin
        {
            get
            {
                return _issiteLogin;
            }
        }

        protected ViewFlags ViewFlags
        {
            get
            {
                return ViewBag.ViewFlags;
            }

            set
            {
                ViewBag.ViewFlags = _viewFlags = value;
            }
        }
    }
}