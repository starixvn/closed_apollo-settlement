// -----------------------------------------------------------------------
// <copyright file="SportsbookController.Refund.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.29">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.Controllers.Settlement
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.Configuration;
    using Fanex.NAP.Settlement.DTO.Domain;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.ViewModels;
    using Fanex.NAP.Settlement.Web.Configuration;

    /// <summary>
    ///     Refund for Sports book.
    /// </summary>
    public partial class SportsbookController
    {
        /// <summary>
        /// Controller refund popup page.
        /// </summary>
        /// <param name="sportId">Sport id value.</param>
        /// <param name="matchId">Match id value.</param>
        /// <param name="actionId">Action id value.</param>
        /// <param name="logId">Log id value.</param>
        /// <returns>View result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookRefund, Policy = Constant.Policy.Read)]
        public ActionResult Refund(int sportId, int matchId, int actionId, int logId)
        {
            var model = new RefundMetaModel
            {
                MatchId = matchId,
                SportTypeId = sportId
            };

            var match = this._calculateService.GetMatchDetail(matchId, true);

            if (actionId > 0 && logId > 0)
            {
                // Set new value action from IBC
                var matchLog = this._calculateService.GetMatchLogDetail(logId);

                match.HtHomeScore = matchLog.HtHomeScore;
                match.HtAwayScore = matchLog.HtAwayScore;
                match.FinalHomeScore = matchLog.FinalHomeScore;
                match.FinalAwayScore = matchLog.FinalAwayScore;
                match.GoalRecord = matchLog.GoalRecord;
            }

            // Default is Soccer
            var sportViewSetting = SportViewSettingFactory.Create(model.SportTypeId);

            var data = new RefundViewModel
            {
                PageType = PageType.Refund,
                HeaderEvent = ViewResources.Team,
                LeagueId = match.LeagueId,
                CurrentMatch = match,
                Template = sportViewSetting.Template,
                DirectlyTemplate = sportViewSetting.DirectlyTemplate,
                GroupSport = sportViewSetting.GroupSport,
            };

            return this.View(data);
        }
    }
}