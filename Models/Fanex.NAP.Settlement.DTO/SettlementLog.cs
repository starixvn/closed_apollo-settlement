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
    using Fanex.NAP.Settlement.DTO.Domain;

    /// <summary>
    ///     Match's Log.
    /// </summary>
    public class SettlementLog
    {
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
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }

        public string Home
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId);
            }
        }

        public string Away
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId);
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