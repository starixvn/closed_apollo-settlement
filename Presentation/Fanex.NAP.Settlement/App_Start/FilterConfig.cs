// --------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.14">Create</change>
// </history>
namespace Fanex.NAP.Settlement.App_Start
{
    #region

    using System.Web.Mvc;

    #endregion

    /// <summary>
    /// Filter Config.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register Global Filters.
        /// </summary>
        /// <param name="filters">Filters parameter.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}