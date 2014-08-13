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
        private const string BetStatusFullTime = "FL:{0}-{1};";
        private const string BetStatusHalfTime = "1FL:{0}-{1};";

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
            var betStatus = SetBetStatusForSoccer(model);

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
            var action = (!string.IsNullOrWhiteSpace(goalRecord) && goalRecord.IndexOf(";", StringComparison.Ordinal) > -1)
                      ? CalculateAction.CalculateFullTime
                      : CalculateAction.CalculateHalfTime;

            var inputParameters = new Hashtable();

            inputParameters.Add("creator", this.CurrentSession.UserId);
            inputParameters.Add("matchid", matchId);
            inputParameters.Add("betstatus", betStatus);
            inputParameters.Add("includelg", false);
            inputParameters.Add("ip", HttpContext.Request.UserHostAddress);
            inputParameters.Add("goalrecord", goalRecord);
            inputParameters.Add("action", action);

            return CalculateService.Calculate(inputParameters, calcType);
        }

        #endregion Calculate first goal and no goal

        #region Set new value for sports

        /// <summary>
        /// Sets the bet status only for soccer.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Bet status string.
        /// </returns>
        private static string SetBetStatusForSoccer(DoSingleTicketMetaModel model)
        {
            return (model.IsExistFgLg || model.IsExistHtFgLg) ? SetNewValueBetStatusForGoalRecord(model) : string.Empty;
        }

        /// <summary>
        /// Bet status value.
        /// </summary>
        /// <param name="timeSetting">Full time (FL) or Half time (1FL).</param>
        /// <param name="goalRecord">Goal record.</param>
        /// <returns>Return bet status value.</returns>
        private static string BetStatusValue(string timeSetting, string goalRecord)
        {
            var firstGoal = 0;
            var lastGoal = 0;
            if (!string.IsNullOrWhiteSpace(goalRecord))
            {
                firstGoal = goalRecord.StartsWith("h", StringComparison.OrdinalIgnoreCase) ? 1 : 2;
                lastGoal = goalRecord.EndsWith("h", StringComparison.OrdinalIgnoreCase) ? 1 : 2;
            }

            return string.Format(timeSetting, firstGoal, lastGoal);
        }

        /// <summary>
        /// Update bet status without semicolon for Calculate FGNG.
        /// </summary>
        /// <param name="criteria">Single Ticket Criteria.</param>
        /// <returns>Bet Status.</returns>
        private static string UpdateBetStatusWithoutSemicolonForCalculateFgng(DoSingleTicketMetaModel criteria)
        {
            var halfTimeBetStatus = string.Empty;
            var fullTimeBetStatus = string.Empty;

            var firstGoal = criteria.GoalRecord.StartsWith("h", StringComparison.OrdinalIgnoreCase) ? 1 : 2;

            if (criteria.IsExistFgLg)
            {
                fullTimeBetStatus = string.Format("FL:{0}-0;", firstGoal);
            }

            if (criteria.IsExistHtFgLg)
            {
                halfTimeBetStatus = string.Format("1FL:{0}-0;", firstGoal);
            }

            return string.Join(string.Empty, halfTimeBetStatus, fullTimeBetStatus);
        }

        /// <summary>
        /// Sets the new value bet status for goal record.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Bet status string.</returns>
        private static string SetNewValueBetStatusForGoalRecord(DoSingleTicketMetaModel model)
        {
            var betStatus = string.Empty;

            switch (model.ActionType.ToLower())
            {
                case CalculationType.CalculateAll:
                    betStatus = SetBetStatusForCalculateAll(model);
                    break;

                case CalculationType.CalculateFgng:
                    betStatus = SetBetStatusForCalculateFgng(model);
                    break;

                case CalculationType.CalculateHalfTime:
                    var halfTimeBetStatus = string.Empty;
                    if (model.IsExistHtFgLg)
                    {
                        halfTimeBetStatus = SetBetStatusForCalculateHalfTime(model);
                    }

                    betStatus = ApplyNewBetStatusForHalfTime(model.BetStatus, halfTimeBetStatus);
                    break;

                case CalculationType.CalculateFullTime:
                    betStatus = SetBetStatusForCalculateFullTime(model);
                    break;
            }

            return betStatus;
        }

        /// <summary>
        /// Sets bet status for calculate full time.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Bet status string.</returns>
        private static string SetBetStatusForCalculateFullTime(DoSingleTicketMetaModel criteria)
        {
            var betStatus = string.Empty;

            if (AreAllScoreAndGoalRecordNull(criteria))
            {
                var isExistFullTime = criteria.BetStatus.IndexOf(";FL", StringComparison.OrdinalIgnoreCase) > -1;
                betStatus = isExistFullTime ? criteria.BetStatus : string.Join(string.Empty, criteria.BetStatus, "FL:0-0;");
            }
            else if (!string.IsNullOrEmpty(criteria.TextBoxFinalAwayScore.ToString()) && !string.IsNullOrEmpty(criteria.TextBoxFinalHomeScore.ToString()) &&
                     !string.IsNullOrEmpty(criteria.GoalRecord.Replace(";", string.Empty)))
            {
                var halfTimeBetStatus = string.Empty;
                var fullTimeBetStatus = string.Empty;

                if (criteria.IsExistHtFgLg)
                {
                    var isExistRefundHT = criteria.BetStatus.IndexOf(EventStatus.RefundHT, StringComparison.OrdinalIgnoreCase) > -1;

                    if (isExistRefundHT)
                    {
                        var isExistHalfTime = criteria.BetStatus.IndexOf("1FL", StringComparison.OrdinalIgnoreCase) > -1;
                        halfTimeBetStatus = isExistHalfTime ? "Refund HT;1FL:0-0;" : "Refund HT;";
                    }
                    else
                    {
                        var goalRecord = criteria.GoalRecord.Split(';')[0];
                        halfTimeBetStatus = BetStatusValue(BetStatusHalfTime, goalRecord);
                    }
                }

                if (criteria.IsExistFgLg)
                {
                    var goalRecord = criteria.GoalRecord.Replace(";", string.Empty);
                    fullTimeBetStatus = BetStatusValue(BetStatusFullTime, goalRecord);
                }

                betStatus = string.Join(string.Empty, halfTimeBetStatus, fullTimeBetStatus);
            }

            return betStatus;
        }

        /// <summary>
        /// Sets bet status for calculate half time.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Bet status string.</returns>
        private static string SetBetStatusForCalculateHalfTime(DoSingleTicketMetaModel criteria)
        {
            var betStatus = string.Empty;

            if (string.IsNullOrEmpty(criteria.TextBoxHtAwayScore.ToString()) && string.IsNullOrEmpty(criteria.TextBoxHtHomeScore.ToString())
                && (string.IsNullOrEmpty(criteria.GoalRecord)
                || (!string.IsNullOrEmpty(criteria.GoalRecord) && string.IsNullOrEmpty(criteria.GoalRecord.Replace(";", string.Empty)))))
            {
                betStatus = "1FL:0-0;";
            }
            else if (IsCalculateHalfTimeWithSemicolon(criteria))
            {
                var goalRecord = criteria.GoalRecord.Split(';')[0];
                betStatus = BetStatusValue(BetStatusHalfTime, goalRecord);
            }
            else if (IsCalculateHalfTimeWithoutSemicolon(criteria))
            {
                var firstGoal = criteria.GoalRecord.StartsWith("h", StringComparison.OrdinalIgnoreCase) ? 1 : 2;
                betStatus = string.Format("1FL:{0}-0;", firstGoal);
            }

            return betStatus;
        }

        /// <summary>
        /// Apply new bet status for half time calculation.
        /// </summary>
        /// <param name="oldBetStatus">Old bet status.</param>
        /// <param name="halfTimeBetStatus">Half time bet status.</param>
        /// <returns>New bet status.</returns>
        private static string ApplyNewBetStatusForHalfTime(string oldBetStatus, string halfTimeBetStatus)
        {
            var betStatus = string.Empty;

            if (string.IsNullOrEmpty(oldBetStatus) || oldBetStatus.IndexOf("1FL", StringComparison.Ordinal) < 0)
            {
                betStatus = halfTimeBetStatus + oldBetStatus;
            }
            else if (oldBetStatus.IndexOf("1FL", StringComparison.Ordinal) > -1)
            {
                var maxLengthHalfTime = 7;
                if (oldBetStatus.Contains(";"))
                {
                    maxLengthHalfTime = 8;
                }

                var oldHalfTimeBetStatus = oldBetStatus.Substring(oldBetStatus.IndexOf("1FL", StringComparison.Ordinal), maxLengthHalfTime);
                betStatus = oldBetStatus.Replace(oldHalfTimeBetStatus, halfTimeBetStatus);
            }

            return betStatus;
        }

        /// <summary>
        /// Update bet status with semicolon for Calculate FGNG.
        /// </summary>
        /// <param name="criteria">Single Ticket Criteria.</param>
        /// <returns>Bet Status.</returns>
        private static string UpdateBetStatusWithSemicolonForCalculateFgng(DoSingleTicketMetaModel criteria)
        {
            var halfTimeBetStatus = string.Empty;
            var fullTimeBetStatus = string.Empty;

            if (criteria.IsExistFgLg)
            {
                var firstGoal = 0;

                if (!string.IsNullOrWhiteSpace(criteria.GoalRecord.Replace(";", string.Empty)))
                {
                    firstGoal = criteria.GoalRecord.StartsWith("h", StringComparison.OrdinalIgnoreCase) ? 1 : 2;
                }

                fullTimeBetStatus = string.Format("FL:{0}-0;", firstGoal);
            }

            if (criteria.IsExistHtFgLg)
            {
                var goalRecord = criteria.GoalRecord.Split(';')[0];
                halfTimeBetStatus = BetStatusValue(BetStatusHalfTime, goalRecord);
            }

            return string.Join(string.Empty, halfTimeBetStatus, fullTimeBetStatus);
        }

        /// <summary>
        /// Sets bet status for calculate first goal no goal.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Bet status string.</returns>
        private static string SetBetStatusForCalculateFgng(DoSingleTicketMetaModel criteria)
        {
            var betStatus = string.Empty;

            if (AreAllScoreAndGoalRecordNull(criteria))
            {
                betStatus = UpdateBetStatusForNoGoal(criteria);
            }
            else if (IsCalculateHalfTimeWithoutSemicolon(criteria))
            {
                betStatus = UpdateBetStatusWithoutSemicolonForCalculateFgng(criteria);
            }
            else if (IsCalculateHalfTimeWithSemicolon(criteria))
            {
                betStatus = UpdateBetStatusWithSemicolonForCalculateFgng(criteria);
            }

            return betStatus;
        }

        /// <summary>
        /// Update bet status for no goal or null bet status case.
        /// </summary>
        /// <param name="criteria">Single Ticket Criteria.</param>
        /// <returns>Bet Status.</returns>
        private static string UpdateBetStatusForNoGoal(DoSingleTicketMetaModel criteria)
        {
            var halfTimeBetStatus = string.Empty;
            var fullTimeBetStatus = string.Empty;

            if (criteria.IsExistFgLg)
            {
                fullTimeBetStatus = "FL:0-0;";
            }

            if (criteria.IsExistHtFgLg)
            {
                halfTimeBetStatus = "1FL:0-0;";
            }

            return string.Join(string.Empty, halfTimeBetStatus, fullTimeBetStatus);
        }

        /// <summary>
        /// Sets bet status for calculate all.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Bet status string.</returns>
        private static string SetBetStatusForCalculateAll(DoSingleTicketMetaModel criteria)
        {
            var betStatus = string.Empty;

            if (AreAllScoreAndGoalRecordNull(criteria))
            {
                betStatus = UpdateBetStatusForNoGoal(criteria);
            }
            else if ((!string.IsNullOrEmpty(criteria.TextBoxFinalAwayScore.ToString()) && !string.IsNullOrEmpty(criteria.TextBoxFinalHomeScore.ToString()) &&
                     !string.IsNullOrEmpty(criteria.GoalRecord) && !criteria.GoalRecord.Contains(";"))
                || ((criteria.TextBoxHtAwayScore >= 0 || criteria.TextBoxHtHomeScore >= 0) && criteria.TextBoxFinalHomeScore >= criteria.TextBoxHtHomeScore &&
                     criteria.TextBoxFinalAwayScore >= criteria.TextBoxHtAwayScore && criteria.GoalRecord.Contains(";")))
            {
                betStatus = UpdateBetStatusForCalculateAll(criteria);
            }

            return betStatus;
        }

        /// <summary>
        /// Update bet status for calculate all.
        /// </summary>
        /// <param name="criteria">Single Ticket Criteria.</param>
        /// <returns>Bet Status.</returns>
        private static string UpdateBetStatusForCalculateAll(DoSingleTicketMetaModel criteria)
        {
            var halfTimeBetStatus = string.Empty;
            var fullTimeBetStatus = string.Empty;

            if (criteria.IsExistHtFgLg)
            {
                var goalRecord = criteria.GoalRecord.Split(';')[0];
                halfTimeBetStatus = BetStatusValue(BetStatusHalfTime, goalRecord);
            }

            if (criteria.IsExistFgLg)
            {
                var goalRecord = criteria.GoalRecord.Replace(";", string.Empty);
                fullTimeBetStatus = BetStatusValue(BetStatusFullTime, goalRecord);
            }

            return string.Join(string.Empty, halfTimeBetStatus, fullTimeBetStatus);
        }

        /// <summary>
        /// Is calculate half time without semicolon case.
        /// </summary>
        /// <param name="criteria">Single ticket criteria.</param>
        /// <returns>Is right or not.</returns>
        private static bool IsCalculateHalfTimeWithoutSemicolon(DoSingleTicketMetaModel criteria)
        {
            return (criteria.TextBoxHtAwayScore >= 0 || criteria.TextBoxHtHomeScore >= 0)
                   && !string.IsNullOrEmpty(criteria.GoalRecord)
                   && !criteria.GoalRecord.Contains(";");
        }

        /// <summary>
        /// Is calculate half time with semicolon case.
        /// </summary>
        /// <param name="criteria">Single ticket criteria.</param>
        /// <returns>Is right or not.</returns>
        private static bool IsCalculateHalfTimeWithSemicolon(DoSingleTicketMetaModel criteria)
        {
            return (criteria.TextBoxHtAwayScore >= 0 || criteria.TextBoxHtHomeScore >= 0)
                   && !string.IsNullOrEmpty(criteria.GoalRecord)
                   && criteria.GoalRecord.Contains(";");
        }

        /// <summary>
        /// Are all score and goal record are null or equal 0.
        /// </summary>
        /// <param name="criteria">Single ticket criteria.</param>
        /// <returns>Are they null or not.</returns>
        private static bool AreAllScoreAndGoalRecordNull(DoSingleTicketMetaModel criteria)
        {
            var isHalfTimeScoreNull = (string.IsNullOrEmpty(criteria.TextBoxHtAwayScore.ToString()) && string.IsNullOrEmpty(criteria.TextBoxHtHomeScore.ToString())) ||
                                      (criteria.TextBoxHtAwayScore.Equals(0) && criteria.TextBoxHtHomeScore.Equals(0));

            var isFullTimeScoreNull = (string.IsNullOrEmpty(criteria.TextBoxFinalAwayScore.ToString()) && string.IsNullOrEmpty(criteria.TextBoxFinalHomeScore.ToString())) ||
                                      (criteria.TextBoxFinalAwayScore.Equals(0) && criteria.TextBoxFinalHomeScore.Equals(0));
            var isAllNull = isHalfTimeScoreNull && isFullTimeScoreNull
                            && (string.IsNullOrEmpty(criteria.GoalRecord)
                                || (!string.IsNullOrEmpty(criteria.GoalRecord) && string.IsNullOrEmpty(criteria.GoalRecord.Replace(";", string.Empty))));

            return isAllNull;
        }

        /// <summary>
        /// Set new value bet status for Goal Record.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <param name="oldBetStatus">Old bet status.</param>
        /// <returns>Return string result.</returns>
        //private static string NewValueBetStatusForGoalRecord(DoSingleTicketMetaModel model, string oldBetStatus)
        //{
        //    var newBetStatus = string.Empty;
        //    if (model.SportId == SportType.Soccer)
        //    {
        //        if (string.IsNullOrEmpty(model.GoalRecord))
        //        {
        //            newBetStatus = "FL:0-0";
        //        }
        //        else
        //        {
        //            // 1 is Home; 2 is Away
        //            var firstGoal = model.GoalRecord.Substring(0, 1).Equals("h", StringComparison.OrdinalIgnoreCase)
        //                                ? "1"
        //                                : "2";
        //            var lastGoal = model.GoalRecord.Substring(model.GoalRecord.Length - 1, 1)
        //                                .Equals("h", StringComparison.OrdinalIgnoreCase)
        //                               ? "1"
        //                               : "2";

        //            if (model.ActionType.Equals("calculatehalftime"))
        //            {
        //                lastGoal = "0";
        //            }

        //            if (oldBetStatus.IndexOf("FL:", StringComparison.Ordinal) > -1)
        //            {
        //                var firstValue = oldBetStatus.Substring(0, oldBetStatus.IndexOf("FL:", StringComparison.Ordinal));
        //                newBetStatus = string.Format("{0}FL:{1}-{2}", firstValue, firstGoal, lastGoal);
        //            }
        //            else
        //            {
        //                newBetStatus = string.IsNullOrWhiteSpace(oldBetStatus)
        //                    ? string.Format("FL:{0}-{1}", firstGoal, lastGoal)
        //                    : string.Format("{0};FL:{1}-{2}", oldBetStatus, firstGoal, lastGoal);
        //                newBetStatus = newBetStatus.Replace(";;", ";");
        //            }
        //        }
        //    }

        //    return newBetStatus;
        //}

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

        ///// <summary>
        ///// Get bet status for calculation.
        ///// </summary>
        ///// <param name="model">Meta model.</param>
        ///// <returns>Return string.</returns>
        //private string BetStatusForCalc(DoSingleTicketMetaModel model)
        //{
        //    Match match = CalculateService.GetMatchDetail(model.MatchId, false);
        //    var isFglg = model.IsExistFgLg;
        //    var betStatus = match.BetStatus ?? string.Empty;

        //    if (!betStatus.Equals(string.Empty))
        //    {
        //        betStatus = betStatus.Replace("Refund HT;", string.Empty);
        //        betStatus = betStatus.Replace("Refund FT;", string.Empty);
        //        betStatus = betStatus.Replace("Refund;", string.Empty);
        //        betStatus = betStatus.Replace("Refund HT", string.Empty);
        //        betStatus = betStatus.Replace("Refund FT", string.Empty);
        //        betStatus = betStatus.Replace("Refund", string.Empty);
        //    }

        //    betStatus = isFglg ? NewValueBetStatusForGoalRecord(model, betStatus) : string.Empty;
        //    return betStatus;
        //}

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
                betStatus = SetBetStatusForSoccer(model);
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
                betStatus = SetBetStatusForSoccer(model);
            }

            var action = model.ActionType.ToLowerInvariant();
            var matchId = model.MatchId;
            var goalRecord = model.SportId == 1 ? model.GoalRecord : string.Empty;
            return this.CalculateCommand(action, matchId, homeHt, homeFt, awayHt, awayFt, betStatus, isHalfTimeCalculate, goalRecord, model.LogId);
        }

        #endregion UnCalculate Ticket
    }
}