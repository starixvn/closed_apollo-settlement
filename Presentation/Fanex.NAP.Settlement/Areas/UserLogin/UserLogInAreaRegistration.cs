// --------------------------------------------------------------------------
// <copyright file="UserLoginAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Settlement.Areas.UserLogin
{
    #region

    using System.Web.Mvc;

    #endregion

    /// <summary>
    /// Area Login.
    /// </summary>
    public class UserLoginAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Area Name.
        /// </summary>
        /// <value>Get area name of login page.</value>
        public override string AreaName
        {
            get { return "UserLogin"; }
        }

        /// <summary>
        /// Register Area.
        /// </summary>
        /// <param name="context">Area Registration Context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_DefaultAreas",
                "UserLogin/{controller}/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional });

            context.MapRoute(this.AreaName + "_Themes", "UserLogin/Themes/{resourceName}");
        }
    }
}