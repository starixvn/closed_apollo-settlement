// -----------------------------------------------------------------------
// <copyright file="BetStatusAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  BetStatus Access Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BetStatusAccess
    {
        private const string FirstGoal = "first-goal";

        private const string LastGoal = "last-goal";

        private const string FirstLastGoal = "first-last-goal";

        private const string HtFirstGoal = "ht-first-goal";

        private const string HtLastGoal = "ht-last-goal";

        private const string HtFirstLastGoal = "ht-first-last-goal";

        private const int ThreeSets = 3;

        private const int FourSets = 4;

        private const int FiveSets = 5;

        /// <summary>
        ///  Initializes a new instance of the <see cref="BetStatusAccess" /> class..
        /// </summary>
        public BetStatusAccess()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetStatusAccess" /> class.
        /// Get bet status data, after that analyze them for different group sports type on the template.
        /// </summary>
        /// <param name="groupSport">Group sports on the template.</param>
        /// <param name="betStatus">Bet status data.</param>
        /// <param name="isExistFglg">Is Exist FGLG.</param>
        /// <param name="isExistHtFglg">Is Exist Half Time FGLG.</param>
        /// <param name="goalRecord">Goal record.</param>
        /// <param name="isFinalScore">Final score.</param>
        public BetStatusAccess(int groupSport, string betStatus, bool isExistFglg, bool isExistHtFglg, string goalRecord, bool isFinalScore)
            : this()
        {
            switch (groupSport)
            {
                // Soccer
                case 1:
                    Soccer(betStatus, isExistFglg, isExistHtFglg, goalRecord, isFinalScore);
                    break;

                // Basketball, Football, Rugby, Netball
                case 2:
                    this.BasketBall(betStatus);
                    break;

                // IceHockey, FiledHockey
                case 3:
                    this.Hockey(betStatus);
                    break;

                // Tennis, Volleyball, Badminton, Squash
                case 4:
                case 7:
                    this.Tennis(betStatus);
                    break;

                // Baseball
                case 5:
                    this.Baseball(betStatus);
                    break;
            }
        }

        public int? Home1Q { get; set; }

        public int? Away1Q { get; set; }

        public int? Home2Q { get; set; }

        public int? Away2Q { get; set; }

        public int? Home3Q { get; set; }

        public int? Away3Q { get; set; }

        public int? Home4Q { get; set; }

        public int? Away4Q { get; set; }

        public int? Home5Q { get; set; }

        public int? Away5Q { get; set; }

        public int? Home6Q { get; set; }

        public int? Away6Q { get; set; }

        public int? Home7Q { get; set; }

        public int? Away7Q { get; set; }

        public int? Home8Q { get; set; }

        public int? Away8Q { get; set; }

        public int? Home9Q { get; set; }

        public int? Away9Q { get; set; }

        public int? HomeOt { get; set; }

        public int? AwayOt { get; set; }

        public int? HomeTotalGames { get; set; }

        public int? AwayTotalGames { get; set; }

        public int? HomeEi { get; set; }

        public int? AwayEi { get; set; }

        public string HomeGoal { get; set; }

        public string AwayGoal { get; set; }

        public string HalfTimeHomeGoal { get; set; }

        public string HalfTimeAwayGoal { get; set; }

        #region Handle BetStatus with other SportType

        #region Soccer

        /// <summary>
        /// Calculate at IBC.
        /// </summary>
        /// <param name="betStatus">Bet status.</param>
        /// <param name="firstGoal">First goal value.</param>
        /// <param name="lastGoal">Last goal value.</param>
        /// <param name="firstLastGoal">First and Last goal value.</param>
        /// <returns>Goal icon.</returns>
        private GoalIcon CalculateByIbc(
                                        string betStatus,
                                        string firstGoal,
                                        string lastGoal,
                                        string firstLastGoal)
        {
            var goalIcon = new GoalIcon();
            var goalValue = betStatus.Split('-');

            // FL: 1-1 -> Home - Home
            if (goalValue[0].Equals("1") && goalValue[1].Equals("1"))
            {
                goalIcon.HomeGoal = firstLastGoal;
            }
            else if (goalValue[0].Equals("1") && goalValue[1].Equals("2"))
            {
                // FL: 1-2 -> Home - Away
                goalIcon.AwayGoal = lastGoal;
                goalIcon.HomeGoal = firstGoal;
            }
            else if (goalValue[0].Equals("2") && goalValue[1].Equals("1"))
            {
                // FL: 2-1 -> Away - Home
                goalIcon.AwayGoal = firstGoal;
                goalIcon.HomeGoal = lastGoal;
            }
            else if (goalValue[0].Equals("2") && goalValue[1].Equals("2"))
            {
                // FL: 2-2-> Away - Away
                goalIcon.AwayGoal = firstLastGoal;
            }
            else if (goalValue[1].Equals("0"))
            {
                // FL: x-0 => x[1,2]: just calc HT
                if (goalValue[0].Equals("1"))
                {
                    // FL: 1-0 -> Home
                    goalIcon.HomeGoal = firstGoal;
                }
                else if (goalValue[0].Equals("2"))
                {
                    // FL: 2-0 -> Away
                    goalIcon.AwayGoal = firstGoal;
                }
            }

            return goalIcon;
        }

        /// <summary>
        /// Calculate Home goal and Away goal.
        /// </summary>
        /// <param name="goalRecord">Goal record text.</param>
        /// <param name="isFinalScore">Final score.</param>
        /// <param name="isExistFglg">Is Exist FGLG.</param>
        /// <param name="isExistHtFglg">Is Exist Half Time FGLG.</param>
        private void CalculateByAdmin(string goalRecord, bool isFinalScore, bool isExistFglg, bool isExistHtFglg)
        {
            if (isExistHtFglg && goalRecord.Contains(";"))
            {
                HandleHalfTimeGoalRecord(goalRecord);
            }

            if (isExistFglg)
            {
                HandleFullTimeGoalRecord(goalRecord, isFinalScore);
            }
        }

        /// <summary>
        /// Update icon for half time home goal and away goal.
        /// </summary>
        /// <param name="goalRecord">Goal record.</param>
        private void HandleHalfTimeGoalRecord(string goalRecord)
        {
            var indexSemicolon = goalRecord.IndexOf(';');

            if (indexSemicolon > -1)
            {
                var halfTimeGoalRecord = goalRecord.Substring(0, indexSemicolon);

                if (halfTimeGoalRecord.StartsWith("a", StringComparison.OrdinalIgnoreCase) && halfTimeGoalRecord.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                {
                    HalfTimeAwayGoal = HtFirstLastGoal;
                }
                else if (halfTimeGoalRecord.StartsWith("h", StringComparison.OrdinalIgnoreCase) && halfTimeGoalRecord.EndsWith("h", StringComparison.OrdinalIgnoreCase))
                {
                    HalfTimeHomeGoal = HtFirstLastGoal;
                }
                else if (halfTimeGoalRecord.StartsWith("a", StringComparison.OrdinalIgnoreCase) && halfTimeGoalRecord.EndsWith("h", StringComparison.OrdinalIgnoreCase))
                {
                    HalfTimeAwayGoal = HtFirstGoal;
                    HalfTimeHomeGoal = HtLastGoal;
                }
                else if (halfTimeGoalRecord.StartsWith("h", StringComparison.OrdinalIgnoreCase) && halfTimeGoalRecord.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                {
                    HalfTimeHomeGoal = HtFirstGoal;
                    HalfTimeAwayGoal = HtLastGoal;
                }
            }
        }

        /// <summary>
        /// Update icon for full time home goal and away goal.
        /// </summary>
        /// <param name="goalRecord">Goal record.</param>
        /// <param name="isFinalScore">Full time goal record.</param>
        private void HandleFullTimeGoalRecord(string goalRecord, bool isFinalScore)
        {
            var fullTimeGoalRecord = goalRecord.Replace(";", string.Empty);

            if (fullTimeGoalRecord.StartsWith("a", StringComparison.OrdinalIgnoreCase))
            {
                AwayGoal = FirstGoal;
            }
            else if (fullTimeGoalRecord.StartsWith("h", StringComparison.OrdinalIgnoreCase))
            {
                HomeGoal = FirstGoal;
            }

            if (isFinalScore)
            {
                if (fullTimeGoalRecord.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                {
                    AwayGoal = string.IsNullOrWhiteSpace(AwayGoal) ? LastGoal : FirstLastGoal;
                }
                else if (fullTimeGoalRecord.EndsWith("h", StringComparison.OrdinalIgnoreCase))
                {
                    HomeGoal = string.IsNullOrWhiteSpace(HomeGoal) ? LastGoal : FirstLastGoal;
                }
            }
        }

        /// <summary>
        ///  SportType Soccer.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        /// <param name="isExistFglg">Is Exist FGLG.</param>
        /// <param name="isExistHtFglg">Is Exist Half Time FGLG.</param>
        /// <param name="goalRecord">Goal Record.</param>
        /// <param name="isFinalScore">Full time.</param>
        private void Soccer(string betStatus, bool isExistFglg, bool isExistHtFglg, string goalRecord, bool isFinalScore)
        {
            if (!string.IsNullOrWhiteSpace(betStatus) && (isExistFglg || isExistHtFglg))
            {
                var indexHalfTime = betStatus.IndexOf("1FL:", StringComparison.Ordinal);

                if (indexHalfTime > -1)
                {
                    var halfTimeBetStatus = betStatus.Substring(indexHalfTime, 7);
                    betStatus = betStatus.Replace(halfTimeBetStatus, string.Empty);

                    if (isExistHtFglg && (!string.IsNullOrWhiteSpace(goalRecord) && goalRecord.Contains(";")))
                    {
                        var halfTimeScore = halfTimeBetStatus.Replace("1FL:", string.Empty);

                        var goalIcon = this.CalculateByIbc(halfTimeScore, HtFirstGoal, HtLastGoal, HtFirstLastGoal);
                        HalfTimeHomeGoal = goalIcon.HomeGoal;
                        HalfTimeAwayGoal = goalIcon.AwayGoal;
                    }
                }

                var indexFullTime = betStatus.IndexOf("FL:", StringComparison.Ordinal);

                if (indexFullTime > -1 && isExistFglg)
                {
                    var fullTimeScore = betStatus.Substring(indexFullTime + 3, 3);
                    var goalIcon = this.CalculateByIbc(fullTimeScore, FirstGoal, LastGoal, FirstLastGoal);
                    HomeGoal = goalIcon.HomeGoal;
                    AwayGoal = goalIcon.AwayGoal;
                }
            }
            else if (!string.IsNullOrWhiteSpace(goalRecord) && (isExistFglg || isExistHtFglg))
            {
                CalculateByAdmin(goalRecord, isFinalScore, isExistFglg, isExistHtFglg);
            }
        }

        #endregion Soccer

        /// <summary>
        ///     SportType BasketBall.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        private void BasketBall(string betStatus)
        {
            var arrayBetStatus = ArrayBetStatus(betStatus);
            if (arrayBetStatus == null)
            {
                return;
            }

            for (var i = 0; i < arrayBetStatus.Length; i++)
            {
                var score = arrayBetStatus[i].Trim().Split('-');
                if (score.Length <= 1)
                {
                    continue;
                }

                if (i < FourSets)
                {
                    ScoreToFourSet(i, score);
                }
                else
                {
                    ScoreToOverTimeSet(score);
                }
            }
        }

        /// <summary>
        ///     SportType Hockey.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        private void Hockey(string betStatus)
        {
            var arrayBetStatus = ArrayBetStatus(betStatus);

            if (arrayBetStatus == null)
            {
                return;
            }

            for (var i = 0; i < arrayBetStatus.Length; i++)
            {
                var score = arrayBetStatus[i].Trim().Split('-');
                if (score.Length <= 1)
                {
                    continue;
                }

                if (i < ThreeSets)
                {
                    ScoreToThreeSet(i, score);
                }
                else
                {
                    ScoreToOverTimeSet(score);
                }
            }
        }

        /// <summary>
        ///     SportType Tennis.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        private void Tennis(string betStatus)
        {
            var arrayBetStatus = ArrayBetStatus(betStatus);
            int totalGameHome = 0, totalGameAway = 0;
            if (arrayBetStatus == null)
            {
                return;
            }

            for (var i = 0; i < arrayBetStatus.Length; i++)
            {
                var score = arrayBetStatus[i].Trim().Split('-');
                if (score.Length <= 1)
                {
                    continue;
                }

                var homeSet = score.Length >= 1
                    ? Convert.ToInt32(arrayBetStatus[i].Trim().Split('-')[0])
                    : 0;
                var awaySet = score.Length >= 2
                    ? Convert.ToInt32(arrayBetStatus[i].Trim().Split('-')[1])
                    : 0;
                if (score.Length >= 1)
                {
                    totalGameHome += homeSet;
                    HomeTotalGames = totalGameHome;
                }

                if (score.Length >= 2)
                {
                    totalGameAway += awaySet;
                    AwayTotalGames = totalGameAway;
                }

                ScoreToFiveSet(i, score);
            }
        }

        /// <summary>
        /// Set score baseball.
        /// </summary>
        /// <param name="i">Index value.</param>
        /// <param name="score">Array score.</param>
        private void SetScoreBaseball(int i, IList<string> score)
        {
            if (i < FiveSets)
            {
                ScoreToFiveSet(i, score);
            }
            else
            {
                switch (i)
                {
                    case 5:
                        Home6Q = Convert.ToInt32(score[0]);
                        Away6Q = Convert.ToInt32(score[1]);
                        break;

                    case 6:
                        Home7Q = Convert.ToInt32(score[0]);
                        Away7Q = Convert.ToInt32(score[1]);
                        break;

                    case 7:
                        Home8Q = Convert.ToInt32(score[0]);
                        Away8Q = Convert.ToInt32(score[1]);
                        break;

                    case 8:
                        Home9Q = Convert.ToInt32(score[0]);
                        Away9Q = Convert.ToInt32(score[1]);
                        break;

                    case 9:
                        HomeEi = Convert.ToInt32(score[0]);
                        AwayEi = Convert.ToInt32(score[1]);
                        break;
                }
            }
        }

        /// <summary>
        ///     SportType Baseball.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        private void Baseball(string betStatus)
        {
            var arrayBetStatus = ArrayBetStatus(betStatus);
            if (arrayBetStatus == null)
            {
                return;
            }

            for (var i = 0; i < arrayBetStatus.Length; i++)
            {
                var score = arrayBetStatus[i].Trim().Split('-');
                if (score.Length > 1)
                {
                    SetScoreBaseball(i, score);
                }
            }
        }

        /// <summary>
        /// Array bet status.
        /// </summary>
        /// <param name="betStatus">Bet status.</param>
        /// <returns>Return array.</returns>
        private string[] ArrayBetStatus(string betStatus)
        {
            return betStatus.IndexOf(';') > -1 && betStatus.Split(';').Length >= 2
                                 ? betStatus.Split(';')[1].Split(',')
                                 : betStatus.Split(',');
        }

        #endregion Handle BetStatus with other SportType

        #region Set score helper

        /// <summary>
        /// Set score to three set.
        /// </summary>
        /// <param name="i">Index value.</param>
        /// <param name="score">Array score.</param>
        private void ScoreToThreeSet(int i, IList<string> score)
        {
            switch (i)
            {
                case 0:
                    Home1Q = Convert.ToInt32(score[0]);
                    Away1Q = Convert.ToInt32(score[1]);
                    break;

                case 1:
                    Home2Q = Convert.ToInt32(score[0]);
                    Away2Q = Convert.ToInt32(score[1]);
                    break;

                case 2:
                    Home3Q = Convert.ToInt32(score[0]);
                    Away3Q = Convert.ToInt32(score[1]);
                    break;
            }
        }

        /// <summary>
        /// Set score to four set.
        /// </summary>
        /// <param name="i">Index value.</param>
        /// <param name="score">Array score.</param>
        private void ScoreToFourSet(int i, IList<string> score)
        {
            if (i < ThreeSets)
            {
                ScoreToThreeSet(i, score);
            }
            else
            {
                Home4Q = Convert.ToInt32(score[0]);
                Away4Q = Convert.ToInt32(score[1]);
            }
        }

        /// <summary>
        /// Set score to five set.
        /// </summary>
        /// <param name="i">Index value.</param>
        /// <param name="score">Array score.</param>
        private void ScoreToFiveSet(int i, IList<string> score)
        {
            if (i < FourSets)
            {
                ScoreToFourSet(i, score);
            }
            else
            {
                Home5Q = Convert.ToInt32(score[0]);
                Away5Q = Convert.ToInt32(score[1]);
            }
        }

        /// <summary>
        /// Set score to over time Set.
        /// </summary>
        /// <param name="score">Array score.</param>
        private void ScoreToOverTimeSet(IList<string> score)
        {
            HomeOt = Convert.ToInt32(score[0]);
            AwayOt = Convert.ToInt32(score[1]);
        }

        #endregion Set score helper
    }
}