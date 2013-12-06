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

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BetStatusAccess
    {
        private const string FirstGoal = "first-goal";

        private const string LastGoal = "last-goal";

        private const string FirstLastGoal = "first-last-goal";

        /// <summary>
        ///  Initializes a new instance of the <see cref="BetStatusAccess" /> class..
        /// </summary>
        public BetStatusAccess()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetStatusAccess"/> class.
        /// Get bet status data, after that analyze them for different group sports type on the template.
        /// </summary>
        /// <param name="teamPort">Group sports on the template.</param>
        /// <param name="betStatus">Bet status data.</param>
        /// <param name="goalRecord">Goal record.</param>
        /// <param name="isFT">Full time.</param>
        public BetStatusAccess(int teamPort, string betStatus, string goalRecord = "", bool isFT = false)
            : this()
        {
            switch (teamPort)
            {
                // Soccer
                case 1:
                    this.Soccer(betStatus, goalRecord, isFT);
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

        #region Handle BetStatus with other SportType

        /// <summary>
        /// Calculate at IBC.
        /// </summary>
        /// <param name="betStatus">Bet status.</param>
        /// <param name="index">Index value.</param>
        private void CalculateAtIBC(string betStatus, int index)
        {
            var homeAwayGoal = betStatus.Substring(index + 3).Split('-');

            // FL: 1-1 -> FL
            if (homeAwayGoal[0] == "1" && homeAwayGoal[1] == "1")
            {
                this.HomeGoal = FirstLastGoal;
            }
            else if (homeAwayGoal[0] == "1" && homeAwayGoal[1] == "2")
            {
                // FL: 1-2 -> Away L; Home F
                this.AwayGoal = LastGoal;
                this.HomeGoal = FirstGoal;
            }
            else if (homeAwayGoal[0] == "2" && homeAwayGoal[1] == "1")
            {
                // FL: 2-1 -> Away F; Home L
                this.AwayGoal = FirstGoal;
                this.HomeGoal = LastGoal;
            }
            else if (homeAwayGoal[0] == "2" && homeAwayGoal[1] == "2")
            {
                // FL: 2-2-> FL
                this.AwayGoal = FirstLastGoal;
            }
            else if (homeAwayGoal[1] == "0")
            {
                // FL: x-0 => x[1,2]: just calc HT
                if (homeAwayGoal[0] == "1")
                {
                    this.HomeGoal = FirstGoal;
                }
                else if (homeAwayGoal[0] == "2")
                {
                    this.AwayGoal = FirstGoal;
                }
            }
            else
            {
                this.HomeGoal = this.AwayGoal = string.Empty;
            }
        }

        /// <summary>
        /// Calculate Home goal and Away goal.
        /// </summary>
        /// <param name="goalRecord">Goal record text.</param>
        /// <param name="fulltime">Is fulltime or not.</param>
        private void CalculateByAdmin(string goalRecord, bool fulltime)
        {
            // Situation Key in HT
            string firstString = goalRecord[0].ToString().ToLower();
            if (firstString == "a")
            {
                this.AwayGoal = FirstGoal;
            }
            else if (firstString == "h")
            {
                this.HomeGoal = FirstGoal;
            }

            // Situation Key in FT
            if (fulltime)
            {
                string lastString = goalRecord[goalRecord.Length - 1].ToString().ToLower();
                if (lastString == "a")
                {
                    if (!string.IsNullOrWhiteSpace(this.AwayGoal))
                    {
                        this.AwayGoal = FirstLastGoal;
                    }
                    else
                    {
                        this.AwayGoal = LastGoal;
                    }
                }
                else if (lastString == "h")
                {
                    if (!string.IsNullOrWhiteSpace(this.HomeGoal))
                    {
                        this.HomeGoal = FirstLastGoal;
                    }
                    else
                    {
                        this.HomeGoal = LastGoal;
                    }
                }
            }
        }

        /// <summary>
        ///  SportType Soccer.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        /// <param name="goalRecord">Goal Record.</param>
        /// <param name="isFT">Full time.</param>
        private void Soccer(string betStatus, string goalRecord = "", bool isFT = false)
        {
            try
            {
                int index = betStatus.IndexOf("FL:", StringComparison.Ordinal);

                if (!string.IsNullOrWhiteSpace(betStatus) && index > -1)
                {
                    this.CalculateAtIBC(betStatus, index);
                }
                else if (!string.IsNullOrWhiteSpace(goalRecord))
                {
                    this.CalculateByAdmin(goalRecord, isFT);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     SportType BasketBall.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        private void BasketBall(string betStatus)
        {
            try
            {
                var arrayBetStatus = this.ArrayBetStatus(betStatus);
                if (arrayBetStatus != null)
                {
                    for (var i = 0; i < arrayBetStatus.Length; i++)
                    {
                        var score = arrayBetStatus[i].Trim().Split('-');
                        if (score.Length >= 2)
                        {
                            switch (i)
                            {
                                case 0:
                                    this.Home1Q = Convert.ToInt32(score[0]);
                                    this.Away1Q = Convert.ToInt32(score[1]);
                                    break;
                                case 1:
                                    this.Home2Q = Convert.ToInt32(score[0]);
                                    this.Away2Q = Convert.ToInt32(score[1]);
                                    break;
                                case 2:
                                    this.Home3Q = Convert.ToInt32(score[0]);
                                    this.Away3Q = Convert.ToInt32(score[1]);
                                    break;
                                case 3:
                                    this.Home4Q = Convert.ToInt32(score[0]);
                                    this.Away4Q = Convert.ToInt32(score[1]);
                                    break;
                                case 4:
                                    this.HomeOt = Convert.ToInt32(score[0]);
                                    this.AwayOt = Convert.ToInt32(score[1]);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     SportType Hockey.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        private void Hockey(string betStatus)
        {
            try
            {
                var arrayBetStatus = this.ArrayBetStatus(betStatus);

                if (arrayBetStatus != null)
                {
                    for (var i = 0; i < arrayBetStatus.Length; i++)
                    {
                        var score = arrayBetStatus[i].Trim().Split('-');
                        if (score.Length >= 2)
                        {
                            switch (i)
                            {
                                case 0:
                                    this.Home1Q = Convert.ToInt32(score[0]);
                                    this.Away1Q = Convert.ToInt32(score[1]);
                                    break;
                                case 1:
                                    this.Home2Q = Convert.ToInt32(score[0]);
                                    this.Away2Q = Convert.ToInt32(score[1]);
                                    break;
                                case 2:
                                    this.Home3Q = Convert.ToInt32(score[0]);
                                    this.Away3Q = Convert.ToInt32(score[1]);
                                    break;
                                case 3:
                                    this.HomeOt = Convert.ToInt32(score[0]);
                                    this.AwayOt = Convert.ToInt32(score[1]);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Set Score for tennis.
        /// </summary>
        /// <param name="i">Index value.</param>
        /// <param name="score">Array score.</param>
        private void SetScoreTennis(int i, string[] score)
        {
            switch (i)
            {
                case 0:
                    this.Home1Q = Convert.ToInt32(score[0]);
                    this.Away1Q = Convert.ToInt32(score[1]);
                    break;
                case 1:
                    this.Home2Q = Convert.ToInt32(score[0]);
                    this.Away2Q = Convert.ToInt32(score[1]);
                    break;
                case 2:
                    this.Home3Q = Convert.ToInt32(score[0]);
                    this.Away3Q = Convert.ToInt32(score[1]);
                    break;
                case 3:
                    this.Home4Q = Convert.ToInt32(score[0]);
                    this.Away4Q = Convert.ToInt32(score[1]);
                    break;
                case 4:
                    this.Home5Q = Convert.ToInt32(score[0]);
                    this.Away5Q = Convert.ToInt32(score[1]);
                    break;
            }
        }

        /// <summary>
        ///     SportType Tennis.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        private void Tennis(string betStatus)
        {
            try
            {
                var arrayBetStatus = this.ArrayBetStatus(betStatus);
                int totalGameHome = 0, totalGameAway = 0;
                if (arrayBetStatus != null)
                {
                    for (var i = 0; i < arrayBetStatus.Length; i++)
                    {
                        var score = arrayBetStatus[i].Trim().Split('-');
                        if (score.Length > 1)
                        {
                            var homeSet = score.Length >= 1
                                              ? Convert.ToInt32(arrayBetStatus[i].Trim().Split('-')[0])
                                              : 0;
                            var awaySet = score.Length >= 2
                                              ? Convert.ToInt32(arrayBetStatus[i].Trim().Split('-')[1])
                                              : 0;
                            if (score.Length >= 1)
                            {
                                totalGameHome += homeSet;
                                this.HomeTotalGames = totalGameHome;
                            }

                            if (score.Length >= 2)
                            {
                                totalGameAway += awaySet;
                                this.AwayTotalGames = totalGameAway;
                            }

                            this.SetScoreTennis(i, score);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Set score baseball.
        /// </summary>
        /// <param name="i">Index value.</param>
        /// <param name="score">Array score.</param>
        private void SetScoreBaseball(int i, string[] score)
        {
            switch (i)
            {
                case 0:
                    this.Home1Q = Convert.ToInt32(score[0]);
                    this.Away1Q = Convert.ToInt32(score[1]);
                    break;
                case 1:
                    this.Home2Q = Convert.ToInt32(score[0]);
                    this.Away2Q = Convert.ToInt32(score[1]);
                    break;
                case 2:
                    this.Home3Q = Convert.ToInt32(score[0]);
                    this.Away3Q = Convert.ToInt32(score[1]);
                    break;
                case 3:
                    this.Home4Q = Convert.ToInt32(score[0]);
                    this.Away4Q = Convert.ToInt32(score[1]);
                    break;
                case 4:
                    this.Home5Q = Convert.ToInt32(score[0]);
                    this.Away5Q = Convert.ToInt32(score[1]);
                    break;
                case 5:
                    this.Home6Q = Convert.ToInt32(score[0]);
                    this.Away6Q = Convert.ToInt32(score[1]);
                    break;
                case 6:
                    this.Home7Q = Convert.ToInt32(score[0]);
                    this.Away7Q = Convert.ToInt32(score[1]);
                    break;
                case 7:
                    this.Home8Q = Convert.ToInt32(score[0]);
                    this.Away8Q = Convert.ToInt32(score[1]);
                    break;
                case 8:
                    this.Home9Q = Convert.ToInt32(score[0]);
                    this.Away9Q = Convert.ToInt32(score[1]);
                    break;
                case 9:
                    this.HomeEi = Convert.ToInt32(score[0]);
                    this.AwayEi = Convert.ToInt32(score[1]);
                    break;
            }
        }

        /// <summary>
        ///     SportType Baseball.
        /// </summary>
        /// <param name="betStatus">Bet Status.</param>
        private void Baseball(string betStatus)
        {
            try
            {
                var arrayBetStatus = this.ArrayBetStatus(betStatus);
                if (arrayBetStatus != null)
                {
                    for (var i = 0; i < arrayBetStatus.Length; i++)
                    {
                        var score = arrayBetStatus[i].Trim().Split('-');
                        if (score.Length > 1)
                        {
                            this.SetScoreBaseball(i, score);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
    }
}