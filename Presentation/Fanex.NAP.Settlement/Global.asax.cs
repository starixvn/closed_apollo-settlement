// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
//   <change who="Kaden.Trinh" date="2013.8.15">Check StyleCop rule</change>
// </history>
namespace Fanex.NAP.Settlement
{
    #region

    using System;
    using System.Configuration;
    using System.IO.Compression;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Data.Configuration;
    using Fanex.NAP.Settlement.App_Start;

    #endregion

    /// <summary>
    /// Note: For instructions on enabling IIS6 or IIS7 classic mode.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Application Start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbSettingProviderManager.Current.Start();
        }
    }
}