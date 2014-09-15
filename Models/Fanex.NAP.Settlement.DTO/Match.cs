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
    using Fanex.NAP.Settlement.Core.Common;
    using Fanex.NAP.Settlement.Core.Constants;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class Match
    {
        private string _homeName = string.Empty;
        private string _awayName = string.Empty;
        private string _leagueName = string.Empty;
        private BetStatusAccess _betStatusAccess;
        private string _eventStatus = string.Empty;

        #region Match Propery

        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_leagueName))
                {
                    this._leagueName = CacheRef.GetLeagueName(LeagueId);
                }

                return this._leagueName;
            }
        }

        public int HomeId { get; set; }

        public string HomeName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_homeName))
                {
                    this._homeName = CacheRef.GetTeamName(HomeId) + (IsNeutral ? " (N)" : string.Empty);
                }

                return _homeName;
            }
        }

        public int AwayId { get; set; }

        public string AwayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_awayName))
                {
                    this._awayName = CacheRef.GetTeamName(AwayId);
                }

                return _awayName;
            }
        }

        public int GroupSport { get; set; }

        public int BetCount { get; set; }

        public byte SportType { get; set; }

        public string MatchCode { get; set; }

        public string BetStatus { get; set; }

        public string GoalRecord { get; set; }

        public string SuspenderHt { get; set; }

        public string HtStatus { get; set; }

        public string FtStatus { get; set; }

        public string FgLgStatus { get; set; }

        public string HtFgLgStatus { get; set; }

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

        public bool IsExistHTFGLG { get; set; }

        public bool IsExistFGLG { get; set; }

        #endregion Match Status

        #region Extra Property

        public string UndecidedColor
        {
            get
            {
                if (FailedActionId.HasValue)
                {
                    return Background.ActionFailed;
                }
                else
                {
                    return this.IsUndecided ? Background.Undecided : string.Empty;
                }
            }
        }

        public string CalcSBBackgroundColor
        {
            get
            {
                return Utility.BackgroundColorCalculate(IsHtCal, IsCalRefund, IsCalculated);
            }
        }

        public string EventStatusColor
        {
            get
            {
                if (this.EventStatus.Equals(Core.Constants.EventStatus.PostPoned, StringComparison.OrdinalIgnoreCase))
                {
                    return Background.Postponed;
                }
                else
                {
                    return this.IsPending ? Background.Pending : string.Empty;
                }
            }
        }

        public BetStatusAccess BetStatusAccess
        {
            get
            {
                if (_betStatusAccess == null)
                {
                    if (!Convert.IsDBNull(BetStatus) && !string.IsNullOrEmpty(BetStatus))
                    {
                        _betStatusAccess = new BetStatusAccess(GroupSport, BetStatus, IsExistFGLG, IsExistHTFGLG, GoalRecord, false);
                    }
                    else if (!string.IsNullOrWhiteSpace(GoalRecord))
                    {
                        var isFinalScore = FinalAwayScore != null && FinalHomeScore != null;

                        _betStatusAccess = new BetStatusAccess(GroupSport, BetStatus, IsExistFGLG, IsExistHTFGLG, GoalRecord, isFinalScore);
                    }
                    else
                    {
                        _betStatusAccess = new BetStatusAccess();
                    }
                }

                return _betStatusAccess;
            }

            set
            {
                _betStatusAccess = value;
            }
        }

        public string EventStatus
        {
            get
            {
                return Utility.MapEventStatusString(_eventStatus, IsPending, IsHtCal, IsCalculated);
            }

            set
            {
                _eventStatus = value;
            }
        }

        #endregion Extra Property

        #region Action from IBC for this match

        public IList<MatchLog> ListAction { get; set; }

        #endregion Action from IBC for this match
    }
}