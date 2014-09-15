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
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.NAP.Settlement.Services.Implement;
    using Fanex.NAP.Settlement.Services.Interface;

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
        protected BaseController()
        {
            UserService = new UserService();
        }

        /// <summary>
        /// Gets or sets UserService.
        /// </summary>
        /// <value>IUserService instance.</value>
        protected IUserService UserService
        {
            get;
            set;
        }

        /// <summary>
        ///     Check permission for function.
        /// </summary>
        /// <param name="userId">User id value.</param>
        /// <param name="functionKey">Function key value.</param>
        /// <param name="policy">Policy value.</param>
        /// <returns>Return true or false.</returns>
        [NonAction]
        protected bool HasPermission(int userId, string functionKey, Constant.Policy policy)
        {
            return userId > 0 ? UserService.HasPermissionUserSiteFunctionKey(userId, NapSettings.Settings.SiteId, functionKey, policy) : false;
        }
    }
}