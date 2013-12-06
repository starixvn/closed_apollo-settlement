// -----------------------------------------------------------------------
// <copyright file="DoSingleTicketController.Calc.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  DoSingleTicket Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.Controllers.TicketController
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.Configuration;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.DTO.Domain;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///    Controller Single ticket Calculate for SportsBook.
    /// </summary>
    public partial class DoSingleTicketController
    {
        #region Calculate Ticket

        /// <summary>
        /// Calculate with model value.
        /// </summary>
        /// <param name="calcType">Calculate type.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <param name="homeHt">Home halftime.</param>
        /// <param name="homeFt">Home fulltime.</param>
        /// <param name="awayHt">Away halftime.</param>
        /// <param name="awayFt">Away fulltime.</param>
        /// <param name="betStatus">Bet status.</param>
        /// <param name="isHtCal">Is Halftime.</param>
        /// <param name="goalRecord">Goal record.</param>
        /// <param name="logId">Log id parameter.</param>
        /// <returns>Return result.</returns>
        public IResult CalculateCommand(
            string calcType = "",
            int matchId = 0,
            int homeHt = 0,
            int homeFt = 0,
            int awayHt = 0,
            int awayFt = 0,
            string betStatus = "",
            bool isHtCal = false,
            string goalRecord = "",
            long logId = 0)
        {
            var inputParameters = new Hashtable();

            inputParameters.Add("creator", CurrentSession.UserId);
            inputParameters.Add("matchid", matchId);
            inputParameters.Add("hth", homeHt);
            inputParameters.Add("hta", awayHt);
            inputParameters.Add("betstatus", betStatus);
            inputParameters.Add("goalrecord", goalRecord);
            inputParameters.Add("ip", HttpContext.Request.UserHostAddress);

            if (!isHtCal)
            {
                inputParameters.Add("fth", homeFt);
                inputParameters.Add("fta", awayFt);
            }

            switch (calcType)
            {
                case CalculationType.CalculateHalfTime:
                    inputParameters.Add("action", CalculateAction.CalculateHalfTime);
                    break;
                case CalculationType.CalculateFullTime:
                    inputParameters.Add("action", CalculateAction.CalculateFullTime);
                    break;
                case CalculationType.Uncalculate:
                    inputParameters.Add("action", CalculateAction.Uncalculate);
                    break;
                default:
                    inputParameters.Add("action", CalculateAction.CalculateAll);
                    break;
            }

            return CalculateService.Calculate(inputParameters, calcType);
        }

        #endregion Calculate Ticket

        #region Calculate first goal and no goal

        /// <summary>
        /// Calculate First Goal and no Goal.
        /// </summary>
        /// <param name="model">Meta model.</param>
        /// <returns>Return result.</returns>
        public IResult CalculateFglg(DoSingleTicketMetaModel model)
        {
            var betStatus = NewValueBetStatusForGoalRecord(model, HttpContext.Request.Form["statusFGLG"]);

            return this.CalculateFglg(model.ActionType.ToLowerInvariant(), model.MatchId, betStatus, model.GoalRecord);
        }

        /// <summary>
        /// Calculate First Goal and no Goal.
        /// </summary>
        /// <param name="calcType">Action type.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <param name="betStatus">Bet status.</param>
        /// <param name="goalRecord">Goal record.</param>
        /// <returns>Return IResult.</returns>
        public IResult CalculateFglg(string calcType = "", int matchId = 0, string betStatus = "", string goalRecord = "")
        {
            var inputParameters = new Hashtable();

            inputParameters.Add("creator", this.CurrentSession.UserId);
            inputParameters.Add("matchid", matchId);
            inputParameters.Add("betstatus", betStatus);
            inputParameters.Add("includelg", false);
            inputParameters.Add("ip", HttpContext.Request.UserHostAddress);
            inputParameters.Add("goalrecord", goalRecord);

            return CalculateService.Calculate(inputParameters, calcType);
        }

        #endregion Calculate first goal and no goal

        #region Set new value for sports

        /// <summary>
        /// Set new value bet status for Goal Record.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="oldBetStatus">Old bet status.</param>
        /// <returns>Return string result.</returns>
        private static string NewValueBetStatusForGoalRecord(DoSingleTicketMetaModel model, string oldBetStatus)
        {
            var newBetStatus = string.Empty;
            if (model.SportId == SportType.Soccer)
            {
                if (string.IsNullOrEmpty(model.GoalRecord))
                {
                    newBetStatus = "FL:0-0";
                }
                else
                {
                    // 1 is Home; 2 is Away
                    var firstGoal = model.GoalRecord.Substring(0, 1).Equals("h", StringComparison.OrdinalIgnoreCase)
                                        ? "1"
                                        : "2";
                    var lastGoal = model.GoalRecord.Substring(model.GoalRecord.Length - 1, 1)
                                        .Equals("h", StringComparison.OrdinalIgnoreCase)
                                       ? "1"
                                       : "2";

                    if (model.ActionType.Equals("calculatehalftime"))
                    {
                        lastGoal = "0";
                    }

                    if (oldBetStatus.IndexOf("FL:", StringComparison.Ordinal) > -1)
                    {
                        var firstValue = oldBetStatus.Substring(0, oldBetStatus.IndexOf("FL:", StringComparison.Ordinal));
                        newBetStatus = string.Format("{0}FL:{1}-{2}", firstValue, firstGoal, lastGoal);
                    }
                    else
                    {
                        newBetStatus = string.IsNullOrWhiteSpace(oldBetStatus)
                            ? string.Format("FL:{0}-{1}", firstGoal, lastGoal)
                            : string.Format("{0};FL:{1}-{2}", oldBetStatus, firstGoal, lastGoal);
                        newBetStatus = newBetStatus.Replace(";;", ";");
                    }
                }
            }

            return newBetStatus;
        }

        /// <summary>
        /// Set new value for soccer sport.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="homeHt">Home halftime.</param>
        /// <param name="awayHt">Away halftime.</param>
        /// <param name="homeFt">Home fulltime.</param>
        /// <param name="awayFt">Away fulltime.</param>
        private void SetValueForSoccer(DoSingleTicketMetaModel model, out int homeHt, out int awayHt, out int homeFt, out int awayFt)
        {
            homeHt = model.TextBoxHtHomeScore == null ? 0 : (int)model.TextBoxHtHomeScore;
            awayHt = model.TextBoxHtAwayScore == null ? 0 : (int)model.TextBoxHtAwayScore;
            homeFt = model.TextBoxFinalHomeScore == null ? 0 : (int)model.TextBoxFinalHomeScore;
            awayFt = model.TextBoxFinalAwayScore == null ? 0 : (int)model.TextBoxFinalAwayScore;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="propertyName">Property Name.</param>
        /// <returns>Return result.</returns>
        private int GetPropertyValue(DoSingleTicketMetaModel model, string propertyName)
        {
            var value = model.GetType().GetProperty(propertyName).GetValue(model, null);

            return value != null ? Convert.ToInt32(value) : 0;
        }

        /// <summary>
        /// Set new value for Billiard, Golf, Motorsports, Winter Sports.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="homeFt">Home fulltime.</param>
        /// <param name="awayFt">Away fulltime.</param>
        private void SetValueForFinalScore(DoSingleTicketMetaModel model, out int homeFt, out int awayFt)
        {
            homeFt = model.TextBoxFinalHomeScore == null ? 0 : (int)model.TextBoxFinalHomeScore;
            awayFt = model.TextBoxFinalAwayScore == null ? 0 : (int)model.TextBoxFinalAwayScore;
        }

        /// <summary>
        /// Set new value for Basketball, football, rugby, netball sports.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="homeHt">Home halftime.</param>
        /// <param name="awayHt">Away halftime.</param>
        /// <param name="homeFt">Home fulltime.</param>
        /// <param name="awayFt">Away fulltime.</param>
        /// <param name="betStatus">Bet status.</param>
        /// <param name="isHalfTime">Is halftime.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
               "StyleCop.CSharp.Nexcel.NexcelCustomRules",
               "SP2100:CodeLineMustNotBeLongerThan",
               Justification = "Reviewed. Suppression is OK here.")]
        private void SetValueForBasketball(DoSingleTicketMetaModel model, out int homeHt, out int awayHt, out int homeFt, out int awayFt, out string betStatus, bool isHalfTime)
        {
            betStatus = string.Empty;
            homeHt = awayHt = homeFt = awayFt = 0;

            if (model.IsScoreDirect)
            {
                this.SetValueForSoccer(model, out homeHt, out awayHt, out homeFt, out awayFt);
            }
            else
            {
                for (var i = 1; i <= 4; i++)
                {
                    var home = this.GetPropertyValue(model, "TextBoxHome" + i + "Q");
                    var away = this.GetPropertyValue(model, "TextBoxAway" + i + "Q");
                    if (isHalfTime)
                    {
                        if (i < 3)
                        {
                            betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);
                        }
                    }
                    else
                    {
                        betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);
                    }

                    if (i < 3)
                    {
                        homeHt += home;
                        awayHt += away;
                    }

                    homeFt += home;
                    awayFt += away;
                }

                if (model.TextBoxHomeOT != null || model.TextBoxAwayOT != null)
                {
                    var home = model.TextBoxHomeOT == null ? 0 : (int)model.TextBoxHomeOT;
                    var away = model.TextBoxAwayOT == null ? 0 : (int)model.TextBoxAwayOT;
                    homeFt += home;
                    awayFt += away;

                    if (!isHalfTime)
                    {
                        betStatus += string.Format(",{0}-{1}", home, away);
                    }
                }
            }
        }

        /// <summary>
        /// Set new value for Hockey, filed Hockey.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="homeHt">Home halftime.</param>
        /// <param name="awayHt">Away halftime.</param>
        /// <param name="homeFt">Home fulltime.</param>
        /// <param name="awayFt">Away fulltime.</param>
        /// <param name="betStatus">Bet status.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
               "StyleCop.CSharp.Nexcel.NexcelCustomRules",
               "SP2100:CodeLineMustNotBeLongerThan",
               Justification = "Reviewed. Suppression is OK here.")]
        private void SetValueForIceHockey(DoSingleTicketMetaModel model, out int homeHt, out int awayHt, out int homeFt, out int awayFt, out string betStatus)
        {
            betStatus = string.Empty;
            homeHt = awayHt = homeFt = awayFt = 0;
            if (model.IsScoreDirect)
            {
                this.SetValueForFinalScore(model, out homeFt, out awayFt);
            }
            else
            {
                for (var i = 1; i < 4; i++)
                {
                    var home = this.GetPropertyValue(model, "TextBoxHome" + i + "Q");
                    var away = this.GetPropertyValue(model, "TextBoxAway" + i + "Q");

                    homeFt += home;
                    awayFt += away;

                    betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);
                }

                if (model.TextBoxHomeOT != null || model.TextBoxAwayOT != null)
                {
                    var home = model.TextBoxHomeOT == null ? 0 : (int)model.TextBoxHomeOT;
                    var away = model.TextBoxAwayOT == null ? 0 : (int)model.TextBoxAwayOT;

                    homeFt += home;
                    awayFt += away;

                    betStatus += string.Format(",{0}-{1}", home, away);
                }
            }
        }

        /// <summary>
        /// Set new value for Tennis, Badminton, Volleyball, Squash.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="homeFt">Home fulltime.</param>
        /// <param name="awayFt">Away fulltime.</param>
        /// <param name="betStatus">Bet status.</param>
        private void SetValueForBadminton(DoSingleTicketMetaModel model, out int homeFt, out int awayFt, out string betStatus)
        {
            betStatus = string.Empty;
            homeFt = awayFt = 0;
            if (model.IsScoreDirect)
            {
                this.SetValueForFinalScore(model, out homeFt, out awayFt);
            }
            else
            {
                for (var i = 1; i < 6; i++)
                {
                    var home = this.GetPropertyValue(model, "TextBoxHome" + i + "Q");
                    var away = this.GetPropertyValue(model, "TextBoxAway" + i + "Q");

                    betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);

                    if (home > away)
                    {
                        homeFt++;
                    }

                    if (home < away)
                    {
                        awayFt++;
                    }
                }
            }
        }

        /// <summary>
        /// Set new value for Baseball.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="homeHt">Home halftime.</param>
        /// <param name="awayHt">Away halftime.</param>
        /// <param name="homeFt">Home fulltime.</param>
        /// <param name="awayFt">Away fulltime.</param>
        /// <param name="betStatus">Bet status.</param>
        /// <param name="isHalfTime">Is halftime.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
               "StyleCop.CSharp.Nexcel.NexcelCustomRules",
               "SP2100:CodeLineMustNotBeLongerThan",
               Justification = "Reviewed. Suppression is OK here.")]
        private void SetValueForBaseball(DoSingleTicketMetaModel model, out int homeHt, out int awayHt, out int homeFt, out int awayFt, out string betStatus, bool isHalfTime)
        {
            betStatus = string.Empty;
            homeHt = awayHt = homeFt = awayFt = 0;
            if (model.IsScoreDirect)
            {
                this.SetValueForSoccer(model, out homeHt, out awayHt, out homeFt, out awayFt);
            }
            else
            {
                for (var i = 1; i < 10; i++)
                {
                    var home = this.GetPropertyValue(model, "TextBoxHome" + i + "Q");
                    var away = this.GetPropertyValue(model, "TextBoxAway" + i + "Q");
                    if (isHalfTime)
                    {
                        if (i < 6)
                        {
                            betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);
                        }
                    }
                    else
                    {
                        betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);
                    }

                    if (i < 6)
                    {
                        homeHt += home;
                        awayHt += away;
                    }

                    homeFt += home;
                    awayFt += away;
                }

                if (model.TextBoxHomeEI != null || model.TextBoxAwayEI != null)
                {
                    var home = model.TextBoxHomeEI == null ? 0 : (int)model.TextBoxHomeEI;
                    var away = model.TextBoxAwayEI == null ? 0 : (int)model.TextBoxAwayEI;

                    if (!isHalfTime)
                    {
                        betStatus += string.Format(",{0}-{1}", home, away);
                    }

                    homeFt += home;
                    awayFt += away;
                }
            }
        }

        #endregion Set new value for sports

        /// <summary>
        ///     Calculate parameter.
        /// </summary>
        /// <param name="model">Single ticket Model.</param>
        /// <param name="homeHt">Half time home team score.</param>
        /// <param name="awayHt">Half time away team score.</param>
        /// <param name="homeFt">Full time home team score.</param>
        /// <param name="awayFt">Full time away team score.</param>
        /// <param name="betStatus">Bet status.</param>
        /// <param name="isHalfTimeCalculate">Half time calculation or not.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
               "StyleCop.CSharp.Nexcel.NexcelCustomRules",
               "SP2100:CodeLineMustNotBeLongerThan",
               Justification = "Reviewed. Suppression is OK here.")]
        private void CalculationParams(DoSingleTicketMetaModel model, out int homeHt, out int awayHt, out int homeFt, out int awayFt, out string betStatus, bool isHalfTimeCalculate)
        {
            homeHt = 0;
            awayHt = 0;
            betStatus = string.Empty;
            switch (model.SportId)
            {
                case SportType.BasketBall:
                case SportType.FootBall:
                case SportType.Rugby:
                case SportType.NetBall:
                    this.SetValueForBasketball(model, out homeHt, out awayHt, out homeFt, out awayFt, out betStatus, isHalfTimeCalculate);
                    break;

                case SportType.IceHockey:
                case SportType.FieldHockey:
                    this.SetValueForIceHockey(model, out homeHt, out awayHt, out homeFt, out awayFt, out betStatus);
                    break;

                case SportType.Tennis:
                case SportType.Badminton:
                case SportType.VolleyBall:
                case SportType.Squash:
                    this.SetValueForBadminton(model, out homeFt, out awayFt, out betStatus);
                    break;

                case SportType.Billard:
                case SportType.Golf:
                case SportType.Motorsports:
                case SportType.Winter:
                    this.SetValueForFinalScore(model, out homeFt, out awayFt);
                    break;

                case SportType.BaseBall:
                    this.SetValueForBaseball(model, out homeHt, out awayHt, out homeFt, out awayFt, out betStatus, isHalfTimeCalculate);
                    break;

                default:
                    this.SetValueForSoccer(model, out homeHt, out awayHt, out homeFt, out awayFt);
                    break;
            }
        }

        /// <summary>
        /// Get bet status for calculation.
        /// </summary>
        /// <param name="model">Meta model.</param>
        /// <returns>Return string.</returns>
        private string BetStatusForCalc(DoSingleTicketMetaModel model)
        {
            Match match = CalculateService.GetMatchDetail(model.MatchId, false);
            var isFglg = model.IsExistFgLg;
            var betStatus = match.BetStatus ?? string.Empty;

            if (!betStatus.Equals(string.Empty))
            {
                betStatus = betStatus.Replace("Refund HT;", string.Empty);
                betStatus = betStatus.Replace("Refund FT;", string.Empty);
                betStatus = betStatus.Replace("Refund;", string.Empty);
                betStatus = betStatus.Replace("Refund HT", string.Empty);
                betStatus = betStatus.Replace("Refund FT", string.Empty);
                betStatus = betStatus.Replace("Refund", string.Empty);
            }

            betStatus = isFglg ? NewValueBetStatusForGoalRecord(model, betStatus) : string.Empty;
            return betStatus;
        }

        /// <summary>
        /// Calculate command with detail value.
        /// </summary>
        /// <param name="model">Single ticket Model.</param>
        /// <returns>Return IResult.</returns>
        private IResult CalculateCommand(DoSingleTicketMetaModel model)
        {
            int homeHt, awayHt, homeFt, awayFt;
            string betStatus;
            var isHalfTimeCalculate = false;

            if (model.ActionType.Equals(CalculationType.CalculateHalfTime, StringComparison.OrdinalIgnoreCase))
            {
                isHalfTimeCalculate = true;
            }

            this.CalculationParams(model, out homeHt, out awayHt, out homeFt, out awayFt, out betStatus, isHalfTimeCalculate);

            // Set bet status for Soccer
            if (model.SportId == SportType.Soccer)
            {
                betStatus = this.BetStatusForCalc(model);
            }

            var action = model.ActionType.ToLowerInvariant();
            var matchId = model.MatchId;
            var goalRecord = model.SportId == 1 ? model.GoalRecord : string.Empty;
            return this.CalculateCommand(action, matchId, homeHt, homeFt, awayHt, awayFt, betStatus, isHalfTimeCalculate, goalRecord);
        }

        #region UnCalculate Ticket

        /// <summary>
        /// Un Calculate.
        /// </summary>
        /// <param name="model">Single ticket Model.</param>
        /// <returns>Return IResult.</returns>
        private IResult UnCalculateCommand(DoSingleTicketMetaModel model)
        {
            int homeHt, awayHt, homeFt, awayFt;
            string betStatus;
            var isHalfTimeCalculate = false;

            this.CalculationParams(model, out homeHt, out awayHt, out homeFt, out awayFt, out betStatus, isHalfTimeCalculate);

            // Set bet status for Soccer
            if (model.SportId == SportType.Soccer)
            {
                betStatus = this.BetStatusForCalc(model);
            }

            var action = model.ActionType.ToLowerInvariant();
            var matchId = model.MatchId;
            var goalRecord = model.SportId == 1 ? model.GoalRecord : string.Empty;
            return this.CalculateCommand(action, matchId, homeHt, homeFt, awayHt, awayFt, betStatus, isHalfTimeCalculate, goalRecord, model.LogId);
        }

        #endregion UnCalculate Ticket
    }
}