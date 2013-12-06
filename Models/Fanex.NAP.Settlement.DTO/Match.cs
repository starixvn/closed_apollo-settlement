// -----------------------------------------------------------------------
// <copyright file="Match.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Match Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DTO
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO.Domain;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class Match
    {
        private readonly string _homeName = string.Empty;
        private readonly string _awayName = string.Empty;
        private readonly string _leagueName = string.Empty;
        private BetStatusAccess _betStatusAccess;

        #region Match Propery

        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_leagueName) ? CacheRef.GetLeagueName(LeagueId) : _leagueName;
            }
        }

        public int HomeId { get; set; }

        public string HomeName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_homeName) ? CacheRef.GetTeamName(HomeId) : _homeName;
            }
        }

        public int AwayId { get; set; }

        public string AwayName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_awayName) ? CacheRef.GetTeamName(AwayId) : _awayName;
            }
        }

        public int GroupSport { get; set; }

        public int BetCount { get; set; }

        public byte SportType { get; set; }

        public string MatchCode { get; set; }

        public string BetStatus { get; set; }

        public string EventStatus { get; set; }

        public string GoalRecord { get; set; }

        public string SuspenderHt { get; set; }

        public string HtStatus { get; set; }

        public string FtStatus { get; set; }

        public string FgLgStatus { get; set; }

        public DateTime KickOffTime { get; set; }

        public DateTime EventDate { get; set; }

        #endregion Match Propery

        #region Match Score

        public int? HtHomeScore { get; set; }

        public int? HtAwayScore { get; set; }

        public int? FinalHomeScore { get; set; }

        public int? FinalAwayScore { get; set; }

        #endregion Match Score

        #region Match Status

        public bool IsKeyScore { get; set; }

        public bool IsCalculated { get; set; }

        public bool IsMultiCaled { get; set; }

        public bool IsRefund { get; set; }

        public bool IsNeutral { get; set; }

        public bool IsCalRefund { get; set; }

        public bool IsHtCal { get; set; }

        public bool IsPending { get; set; }

        public bool IsUndecided { get; set; }

        public long LogId { get; set; }

        public byte? FailedActionId { get; set; }

        public bool DisabledAutoSettle { get; set; }

        #endregion Match Status

        #region Extra Property

        public BetStatusAccess BetStatusAccess
        {
            get
            {
                if (this._betStatusAccess == null)
                {
                    this._betStatusAccess = new BetStatusAccess();

                    if (!Convert.IsDBNull(this.BetStatus)
                        && !string.IsNullOrEmpty(this.BetStatus)
                        && this.SportType == 1)
                    {
                        if (this.BetStatus.IndexOf("FL:", StringComparison.Ordinal) > -1)
                        {
                            var homeAwayGoal = this.BetStatus.Substring(this.BetStatus.IndexOf("FL:", StringComparison.Ordinal) + 3);
                            var strhwgoals = homeAwayGoal.Split('-');
                            int homegoal = Convert.ToInt32(strhwgoals[0]);
                            int awayhome = Convert.ToInt32(strhwgoals[1]);
                            int total = homegoal + awayhome;
                            if (total > 0)
                            {
                                this._betStatusAccess = new BetStatusAccess(this.GroupSport, this.BetStatus);
                            }
                        }
                    }
                    else if (string.IsNullOrEmpty(this.BetStatus) && !string.IsNullOrEmpty(GoalRecord) && this.SportType == 1)
                    {
                        bool isFullTime = false;
                        if (this.FinalAwayScore != null && this.FinalHomeScore != null)
                        {
                            isFullTime = true;
                        }

                        this._betStatusAccess = new BetStatusAccess(this.GroupSport, this.BetStatus, GoalRecord, isFullTime);
                    }
                    else if (!Convert.IsDBNull(this.BetStatus) && !string.IsNullOrEmpty(this.BetStatus))
                    {
                        this._betStatusAccess = new BetStatusAccess(this.GroupSport, this.BetStatus);
                    }
                }

                return this._betStatusAccess;
            }
        }

        #endregion Extra Property

        #region Action from IBC for this match

        public IList<MatchLog> ListAction { get; set; }

        #endregion Action from IBC for this match
    }
}