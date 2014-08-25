// <history>
//  <change who="Kaden" date="2014.08.15">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.Controllers.Helper
{
    using System;
    using Fanex.Common;
    using Fanex.NAP.Settlement.Core.Constants;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    /// Calculation helper.
    /// </summary>
    public static class CalculationHelper
    {
        private const string Home = "Home";
        private const string Away = "Away";
        private const string Q = "Q";
        private const int NumberSetOfBasketball = 4;
        private const int NumberSetOfBaseball = 10;
        private const int NumberSetOfBadminton = 6;
        private const string BetStatusFullTime = "FL:{0}-{1};";
        private const string BetStatusHalfTime = "1FL:{0}-{1};";

        /// <summary>
        /// Set bet status value.
        /// </summary>
        /// <param name="model">The score value.</param>
        /// <param name="isHalfTimeCalculate">HalfTime Calculate.</param>
        public static void CalculateScoreAndBetStatus(DoSingleTicketMetaModel model, bool isHalfTimeCalculate)
        {
            switch (model.SportId)
            {
                case SportType.Soccer:
                    model.BetStatus = CalculationHelper.SetBetStatusForSoccer(model);
                    break;

                case SportType.BasketBall:
                case SportType.FootBall:
                case SportType.Rugby:
                case SportType.NetBall:
                    CalculationHelper.SetValueForBasketball(model, isHalfTimeCalculate);
                    break;

                case SportType.IceHockey:
                case SportType.FieldHockey:
                    CalculationHelper.SetValueForIceHockey(model);
                    break;

                case SportType.Tennis:
                case SportType.Badminton:
                case SportType.VolleyBall:
                case SportType.Squash:
                    CalculationHelper.SetValueForBadminton(model);
                    break;

                case SportType.BaseBall:
                    CalculationHelper.SetValueForBaseball(model, isHalfTimeCalculate);
                    break;
            }
        }

        /// <summary>
        /// Sets the bet status only for soccer.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Bet status string.
        /// </returns>
        public static string SetBetStatusForSoccer(DoSingleTicketMetaModel criteria)
        {
            return (criteria.IsExistFgLg || criteria.IsExistHtFgLg) ? SetNewValueBetStatusForGoalRecord(criteria) : string.Empty;
        }

        /// <summary>
        /// Setting up the score for each set of Baseball.
        /// If you calculate for half time, the system will set value for five set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="isHalfTimeCalculate">If set to <c>true</c> [is half time calculate].</param>
        public static void SetValueForBaseball(DoSingleTicketMetaModel criteria, bool isHalfTimeCalculate)
        {
            var betStatus = string.Empty;
            int homeHt = 0, awayHt = 0, homeFt = 0, awayFt = 0;
            for (var i = 1; i < NumberSetOfBaseball; i++)
            {
                var home = GetPropertyValue(criteria, Home + i + Q);
                var away = GetPropertyValue(criteria, Away + i + Q);

                if (!isHalfTimeCalculate)
                {
                    betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);
                }
                else if (i < 6)
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

            if (criteria.HomeEI.HasValue || criteria.AwayEI.HasValue)
            {
                var homeEi = criteria.HomeEI.HasValue ? criteria.HomeEI.Value : 0;
                var awayEi = criteria.AwayEI.HasValue ? criteria.AwayEI.Value : 0;

                if (!isHalfTimeCalculate)
                {
                    betStatus += string.Format(",{0}-{1}", homeEi, awayEi);
                }

                homeFt += homeEi;
                awayFt += awayEi;
            }

            // Assign score and bet status to criteria.
            SetValueForScoreAndBetStatus(criteria, betStatus, homeFt, awayFt, homeHt, awayHt);
        }

        /// <summary>
        /// Setting up the score for each set of Badminton.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public static void SetValueForBadminton(DoSingleTicketMetaModel criteria)
        {
            var betStatus = string.Empty;
            int homeFt = 0, awayFt = 0;
            for (var i = 1; i < NumberSetOfBadminton; i++)
            {
                var home = GetPropertyValue(criteria, Home + i + Q);
                var away = GetPropertyValue(criteria, Away + i + Q);

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

            // Assign score and bet status to criteria.
            SetValueForScoreAndBetStatus(criteria, betStatus, homeFt, awayFt, 0, 0);
        }

        /// <summary>
        /// Setting up the score for each set of ice hockey.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public static void SetValueForIceHockey(DoSingleTicketMetaModel criteria)
        {
            var betStatus = string.Empty;
            int homeHt = 0, awayHt = 0, homeFt = 0, awayFt = 0;
            for (var i = 1; i < NumberSetOfBasketball; i++)
            {
                var home = GetPropertyValue(criteria, Home + i + Q);
                var away = GetPropertyValue(criteria, Away + i + Q);

                homeFt += home;
                awayFt += away;

                betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);
            }

            if (criteria.HomeOT.HasValue || criteria.AwayOT.HasValue)
            {
                var overtimeHome = criteria.HomeOT.HasValue ? criteria.HomeOT.Value : 0;
                var overtimeAway = criteria.AwayOT.HasValue ? criteria.AwayOT.Value : 0;

                homeFt += overtimeHome;
                awayFt += overtimeAway;

                betStatus += string.Format(",{0}-{1}", overtimeHome, overtimeAway);
            }

            // Assign score and bet status to criteria.
            SetValueForScoreAndBetStatus(criteria, betStatus, homeFt, awayFt, homeHt, awayHt);
        }

        /// <summary>
        /// Setting up the score for each set of basketball.
        /// If you calculate for half time, the system will set value for two set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="isHalfTimeCalculate">If set to <c>true</c> [is half time calculate].</param>
        public static void SetValueForBasketball(DoSingleTicketMetaModel criteria, bool isHalfTimeCalculate)
        {
            var betStatus = string.Empty;
            int homeHt = 0, awayHt = 0, homeFt = 0, awayFt = 0;
            for (var i = 1; i <= NumberSetOfBasketball; i++)
            {
                var home = GetPropertyValue(criteria, Home + i + Q);
                var away = GetPropertyValue(criteria, Away + i + Q);

                if (!isHalfTimeCalculate)
                {
                    betStatus += string.Format("{0}{1}-{2}", !string.IsNullOrEmpty(betStatus) ? "," : string.Empty, home, away);
                }
                else if (i < 3)
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

            if (criteria.HomeOT.HasValue || criteria.AwayOT.HasValue)
            {
                var overtimeHome = criteria.HomeOT.HasValue ? criteria.HomeOT.Value : 0;
                var overtimeAway = criteria.AwayOT.HasValue ? criteria.AwayOT.Value : 0;

                homeFt += overtimeHome;
                awayFt += overtimeAway;

                if (!isHalfTimeCalculate)
                {
                    betStatus += string.Format(",{0}-{1}", overtimeHome, overtimeAway);
                }
            }

            // Assign score and bet status to criteria.
            SetValueForScoreAndBetStatus(criteria, betStatus, homeFt, awayFt, homeHt, awayHt);
        }

        /// <summary>
        /// Sets the value for score and bet status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="betStatus">The bet status.</param>
        /// <param name="homeFt">The home full time.</param>
        /// <param name="awayFt">The away full time.</param>
        /// <param name="homeHt">The home half time.</param>
        /// <param name="awayHt">The away half time.</param>
        private static void SetValueForScoreAndBetStatus(
            DoSingleTicketMetaModel criteria,
            string betStatus,
            int homeFt,
            int awayFt,
            int homeHt,
            int awayHt)
        {
            criteria.BetStatus = betStatus;
            criteria.FinalHomeScore = homeFt;
            criteria.FinalAwayScore = awayFt;
            criteria.HtHomeScore = homeHt;
            criteria.HtAwayScore = awayHt;
        }

        /// <summary>
        /// Get value of a property of model.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>
        /// Return integer result.
        /// </returns>
        private static int GetPropertyValue(DoSingleTicketMetaModel criteria, string propertyName)
        {
            var value = criteria.GetType().GetProperty(propertyName).GetValue(criteria, null);
            return Converter.ToInt(value);
        }

        /// <summary>
        /// Reset new value for bet status.
        /// </summary>
        /// <param name="betStatus">Old bet status value.</param>
        /// <returns>Return new bet status.</returns>
        private static string SetBetStatusScoreAndGoalRecordNull(string betStatus)
        {
            var newBetStatus = string.Empty;
            var isExistHalfTimeFirstGoalLastGoal = betStatus.IndexOf("1FL", StringComparison.OrdinalIgnoreCase) > -1;

            if (isExistHalfTimeFirstGoalLastGoal)
            {
                var indexFullTime = betStatus.IndexOf(";FL", StringComparison.OrdinalIgnoreCase);
                if (indexFullTime > -1)
                {
                    var fullTimeValue = betStatus.Substring(indexFullTime + 1, 7);
                    newBetStatus = betStatus.Replace(fullTimeValue, string.Format(BetStatusFullTime, 0, 0));
                }
                else
                {
                    newBetStatus = string.Join(string.Empty, betStatus, string.Format(BetStatusFullTime, 0, 0));
                }
            }
            else
            {
                var indexFullTime = betStatus.IndexOf("FL", StringComparison.OrdinalIgnoreCase);
                if (indexFullTime > -1)
                {
                    var fullTimeValue = betStatus.Substring(indexFullTime, 7);
                    newBetStatus = betStatus.Replace(fullTimeValue, string.Format(BetStatusFullTime, 0, 0));
                }
                else
                {
                    newBetStatus = string.Join(string.Empty, betStatus, string.Format(BetStatusFullTime, 0, 0));
                }
            }

            return newBetStatus;
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
                betStatus = SetBetStatusScoreAndGoalRecordNull(criteria.BetStatus);
            }
            else if (!string.IsNullOrEmpty(criteria.FinalAwayScore.ToString()) && !string.IsNullOrEmpty(criteria.FinalHomeScore.ToString()) &&
                     !string.IsNullOrEmpty(criteria.GoalRecord.Replace(";", string.Empty)))
            {
                var halfTimeBetStatus = string.Empty;
                var fullTimeBetStatus = string.Empty;
                var isExistRefundHT = criteria.BetStatus.IndexOf(EventStatus.RefundHT, StringComparison.OrdinalIgnoreCase) > -1;

                if (criteria.IsExistHtFgLg)
                {
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

                    if (isExistRefundHT && halfTimeBetStatus.IndexOf(EventStatus.RefundHT, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        fullTimeBetStatus = "Refund HT;";
                    }

                    fullTimeBetStatus += BetStatusValue(BetStatusFullTime, goalRecord);
                }

                betStatus = string.Join(string.Empty, halfTimeBetStatus, fullTimeBetStatus);
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
        /// Sets bet status for calculate half time.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Bet status string.</returns>
        private static string SetBetStatusForCalculateHalfTime(DoSingleTicketMetaModel criteria)
        {
            var betStatus = string.Empty;

            if (string.IsNullOrEmpty(criteria.HtAwayScore.ToString()) && string.IsNullOrEmpty(criteria.HtHomeScore.ToString())
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
        /// Is calculate half time without semicolon case.
        /// </summary>
        /// <param name="criteria">Single ticket criteria.</param>
        /// <returns>Is right or not.</returns>
        private static bool IsCalculateHalfTimeWithoutSemicolon(DoSingleTicketMetaModel criteria)
        {
            return (criteria.HtAwayScore >= 0 || criteria.HtHomeScore >= 0)
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
            return (criteria.HtAwayScore >= 0 || criteria.HtHomeScore >= 0)
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
            var isHalfTimeScoreNull = (string.IsNullOrEmpty(criteria.HtAwayScore.ToString()) && string.IsNullOrEmpty(criteria.HtHomeScore.ToString())) ||
                                      (criteria.HtAwayScore.Equals(0) && criteria.HtHomeScore.Equals(0));

            var isFullTimeScoreNull = (string.IsNullOrEmpty(criteria.FinalAwayScore.ToString()) && string.IsNullOrEmpty(criteria.FinalHomeScore.ToString())) ||
                                      (criteria.FinalAwayScore.Equals(0) && criteria.FinalHomeScore.Equals(0));
            var isAllNull = isHalfTimeScoreNull && isFullTimeScoreNull
                            && (string.IsNullOrEmpty(criteria.GoalRecord)
                                || (!string.IsNullOrEmpty(criteria.GoalRecord) && string.IsNullOrEmpty(criteria.GoalRecord.Replace(";", string.Empty))));

            return isAllNull;
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
            else if ((!string.IsNullOrEmpty(criteria.FinalAwayScore.ToString()) && !string.IsNullOrEmpty(criteria.FinalHomeScore.ToString()) &&
                     !string.IsNullOrEmpty(criteria.GoalRecord) && !criteria.GoalRecord.Contains(";"))
                || ((criteria.HtAwayScore >= 0 || criteria.HtHomeScore >= 0) && criteria.FinalHomeScore >= criteria.HtHomeScore &&
                     criteria.FinalAwayScore >= criteria.HtAwayScore && criteria.GoalRecord.Contains(";")))
            {
                betStatus = UpdateBetStatusForCalculateAll(criteria);
            }

            return betStatus;
        }

        /// <summary>
        /// Sets the new value bet status for goal record.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Bet status string.</returns>
        private static string SetNewValueBetStatusForGoalRecord(DoSingleTicketMetaModel criteria)
        {
            var betStatus = string.Empty;

            switch (criteria.ActionType.ToLower())
            {
                case CalculationType.CalculateAll:
                    betStatus = SetBetStatusForCalculateAll(criteria);
                    break;

                case CalculationType.CalculateFgng:
                    betStatus = SetBetStatusForCalculateFgng(criteria);
                    break;

                case CalculationType.CalculateHalfTime:
                    var halfTimeBetStatus = string.Empty;
                    if (criteria.IsExistHtFgLg)
                    {
                        halfTimeBetStatus = SetBetStatusForCalculateHalfTime(criteria);
                    }

                    betStatus = ApplyNewBetStatusForHalfTime(criteria.BetStatus, halfTimeBetStatus);
                    break;

                case CalculationType.CalculateFullTime:
                    betStatus = SetBetStatusForCalculateFullTime(criteria);
                    break;
            }

            return betStatus;
        }
    }
}