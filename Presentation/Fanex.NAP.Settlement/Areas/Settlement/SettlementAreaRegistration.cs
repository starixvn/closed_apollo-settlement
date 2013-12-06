// --------------------------------------------------------------------------
// <copyright file="SettlementAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.14">Support bundle mode release</change>
// </history>
namespace Fanex.NAP.Settlement.Areas.Settlement
{
    #region

    using System.Web.Mvc;

    #endregion

    /// <summary>
    /// Settlement Area Registration.
    /// </summary>
    public class SettlementAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Area Name.
        /// </summary>
        /// <value>Get area name of Settlement page.</value>
        public override string AreaName
        {
            get { return "Settlement"; }
        }

        /// <summary>
        /// Register Area.
        /// </summary>
        /// <param name="context">Area Registration Context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_DefaultAreas",
                this.AreaName + "/{controller}/{action}/{id}",
                new
                {
                    controller = "Sportsbook",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new[] { "Fanex.NAP.Settlement.Controllers.Settlement" });
        }
    }
}