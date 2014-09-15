// -----------------------------------------------------------------------
// <copyright file="SettlementLog.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  DTO SettlementLog
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.DTO
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.Core.Constants;

    /// <summary>
    ///     Match's Log.
    /// </summary>
    public class SettlementLog
    {
        private string _leagueName = string.Empty;
        private string _homeName = string.Empty;
        private string _awayName = string.Empty;

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int LeagueId { get; set; }

        public bool IsNeutral { get; set; }

        public string ShowTime { get; set; }

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

        public string Home
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_homeName))
                {
                    this._homeName = CacheRef.GetTeamName(HomeId);
                }

                return _homeName;
            }
        }

        public string Away
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

        public string MatchName
        {
            get
            {
                return string.Join(" -vs- ", Home, Away);
            }
        }

        public int SportType { get; set; }

        public DateTime EventDate { get; set; }

        public IList<SettlementLogDetails> MatchLogDetailsList { get; set; }
    }
}