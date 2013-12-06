// -----------------------------------------------------------------------
// <copyright file="UserControlController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.NAP.Settlement.Services.Implement;
    using Fanex.NAP.Settlement.Services.Interface;
    using MetaModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserControlController : BaseController
    {
        #region Fields

        /// <summary>
        ///    Gets or sets  League Service.
        /// </summary>
        private readonly ILeagueService _leagueService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlController" /> class.
        /// </summary>
        public UserControlController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlController" /> class.
        /// </summary>
        /// <param name="leagueService">League service.</param>
        public UserControlController(ILeagueService leagueService)
        {
            this._leagueService = leagueService ?? new LeagueService();
        }

        #endregion Constructor

        /// <summary>
        /// Re-load Leagues list when change sport type.
        /// </summary>
        /// <param name="sportTypeId">Sport type id.</param>
        /// <param name="fromDate">From date parameter.</param>
        /// <param name="toDate">To date parameter.</param>
        /// <param name="showAll">Show all parameter.</param>
        /// <param name="isOutright">Outright parameter.</param>
        /// <returns>Return result.</returns>
        [HttpPost]
        public ActionResult LeaguesList(int sportTypeId, DateTime fromDate, DateTime toDate, bool showAll = true, bool isOutright = false)
        {
            // Get default all league by sport
            var leaguesList = this._leagueService.GetLeagueList(new LeagueMetaModel
            {
                SportTypeId = sportTypeId,
                FromDate = fromDate,
                ToDate = toDate,
                IsOutright = isOutright
            });

            return showAll
                    ? this.View("~/Areas/UserControl/LeaguesList.cshtml", leaguesList)
                    : this.View("~/Areas/UserControl/LeaguesListNotShowAll.cshtml", leaguesList);
        }

        /// <summary>
        /// Get list status.
        /// </summary>
        /// <param name="hideAll">Hide all parameter.</param>
        /// <param name="statusId">Status id parameter.</param>
        /// <returns>Return View result.</returns>
        [HttpPost]
        public ActionResult StatusList(bool hideAll, int statusId)
        {
            var data = string.Format("{0}|{1}", hideAll, statusId);
            return this.View("~/Areas/UserControl/StatusList.cshtml", (object)data);
        }
    }
}