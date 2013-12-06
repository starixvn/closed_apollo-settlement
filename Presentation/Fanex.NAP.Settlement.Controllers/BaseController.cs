// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Base Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.Controllers
{
    #region

    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [HandleError]
    public class BaseController : Controller
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseController" /> class.
        /// </summary>
        public BaseController()
        {
            this.UserService = new UserService();
            this.CurrentSession = new CurrentSession();
        }

        #region Fields

        /// <summary>
        /// Gets or sets UserService.
        /// </summary>
        /// <value>The user service of base controller.</value>
        public IUserService UserService { get; set; }

        /// <summary>
        /// Gets or sets CurrentSession.
        /// </summary>
        /// <value>The current session of base controller.</value>
        public ICurrentSession CurrentSession { get; set; }

        #endregion Fields
    }
}