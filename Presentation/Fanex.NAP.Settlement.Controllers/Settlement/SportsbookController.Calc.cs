// -----------------------------------------------------------------------
// <copyright file="SportsbookController.Calc.cs" company="Nexcel Solutions Vietnam">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Core.Web.Attribute;
    using DTO;
    using DTO.Configuration;
    using DTO.Domain;
    using DTO.Permission;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Settlement.Web.Common;
    using Fanex.NAP.Settlement.Web.Services;
    using ViewModels;
    using Web.Configuration;

    /// <summary>
    /// Partial class Sports book.
    /// </summary>
    public partial class SportsbookController
    {
        /// <summary>
        ///     Sport list show full button.
        /// </summary>
        private static readonly IList<int> FullButtonSportList = new List<int>
        {
            SportType.Soccer,
            SportType.BaseBall,
            SportType.FootBall,
            SportType.Rugby,
            SportType.NetBall,
            SportType.BasketBall
        };

        private static readonly IList<int> TennisGroupSportId = new List<int>
        {
            5, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 99
        };

        /// <summary>
        ///     Controller calculation popup page when click on reference action.
        /// </summary>
        /// <param name="sportId">Sport type.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <param name="actionId">Action Id from IBC.</param>
        /// <param name="logId">Log Id parameter.</param>
        /// <returns>Return Action result.</returns>
        [HttpGet]
        [OutputCache(Duration = 0, VaryByParam = "none")]
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookSettlement)]
        public ActionResult Calc(int sportId, int matchId, int actionId, int logId)
        {
            var match = this._calculateService.GetMatchDetail(matchId, false);

            if (logId > 0)
            {
                // Set new value action from IBC
                var matchLog = this._calculateService.GetMatchLogDetail(logId);

                match.HtHomeScore = matchLog.HtHomeScore;
                match.HtAwayScore = matchLog.HtAwayScore;
                match.FinalHomeScore = matchLog.FinalHomeScore;
                match.FinalAwayScore = matchLog.FinalAwayScore;
                match.BetStatus = matchLog.BetStatus;
                match.GoalRecord = matchLog.GoalRecord;
            }

            var eventStatus = Utility.MapEventStatusString(match.EventStatus, match.IsPending, match.IsHtCal, match.IsCalculated);
            if (this.IsMatchReadyForCalc(match) || eventStatus.Equals(EventStatus.Completed, StringComparison.OrdinalIgnoreCase))
            {
                // Full buttons: Soccer, Basketball, Football, Rugby, Netball, Baseball
                var showFullButton = FullButtonSportList.Contains(match.SportType);

                // Default is Soccer
                var sportViewSetting = SportViewSettingFactory.Create(sportId);

                var data = new CalcViewModel
                {
                    PageType = PageType.Calc,
                    HeaderEvent = ViewResources.Team,
                    LeagueId = match.LeagueId,
                    CurrentMatch = match,
                    ShowFullButton = showFullButton,
                    GroupSport = sportViewSetting.GroupSport,
                    Template = sportViewSetting.Template,
                    DirectlyTemplate = sportViewSetting.DirectlyTemplate,
                    Permission = new SettlementPermission
                    {
                        SportsBook = new SportsBookPermission
                        {
                            CanUncalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookUncalculate),
                            CanCalculate = PermissionService.HasReadPermission(SettlementFunctionKeys.SportsbookCalculate)
                        }
                    }
                };

                return this.View(data);
            }

            return this.View("~/Areas/Settlement/Views/UserControl/UnreadyMatch.cshtml", (object)ViewResources.NotReadyMatchError);
        }

        /// <summary>
        ///     Check if match is ready for calculation.
        ///     This function is used to replace [NAP_Set_MatchCheckForBill].
        /// </summary>
        /// <param name="match">Match detail.</param>
        /// <returns>true or false.</returns>
        private bool IsMatchReadyForCalc(MatchDetail match)
        {
            var soccerHtMinute = 45;
            var basketBallMinute = 30;
            var hockeyMinute = 60;
            var tennisMinute = 30;
            var sportType = match.SportType;

            if (match.ConfirmFt > 0 || match.ConfirmHt > 0)
            {
                return true;
            }

            // Bill soccer HT
            if (sportType == SportType.Soccer || sportType == SportType.VirtualSport)
            {
                return match.PlayTime > soccerHtMinute;
            }

            // Bill Basket, Football HT
            if (sportType == SportType.BasketBall || sportType == SportType.FootBall || sportType == SportType.Rugby || sportType == SportType.NetBall)
            {
                return match.PlayTime > basketBallMinute;
            }

            if (sportType == SportType.IceHockey || sportType == SportType.VolleyBall || sportType == SportType.BaseBall)
            {
                return match.PlayTime >= hockeyMinute;
            }

            if (TennisGroupSportId.Contains(sportType))
            {
                return match.PlayTime > tennisMinute;
            }

            return true;
        }
    }
}