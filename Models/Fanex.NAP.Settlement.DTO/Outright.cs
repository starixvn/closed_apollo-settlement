// -----------------------------------------------------------------------
// <copyright file="Outright.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Outright Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created</change>
//  <change who="Kaden" date="2013.11.26">Add more properties.</change>
// </history>
namespace Fanex.NAP.Settlement.DTO
{
    using System;
    using Fanex.NAP.Settlement.Core.Constants;

    /// <summary>
    /// Class Outright.
    /// </summary>
    public class Outright
    {
        private string _leagueName = string.Empty;
        private string _teamName = string.Empty;
        private string _matchCode;

        public int OrId { get; set; }

        public int LeagueId { get; set; }

        public string TeamName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_teamName))
                {
                    this._teamName = CacheRef.GetTeamName(TeamId);
                }

                return this._teamName;
            }
        }

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

        public int TeamId { get; set; }

        public string MatchCode
        {
            get
            {
                return string.Format("Code : {0}", this._matchCode);
            }

            set
            {
                this._matchCode = value;
            }
        }

        public DateTime KickOffTime { get; set; }

        public DateTime EventDate { get; set; }

        public string OddsStatus { get; set; }

        public string Status { get; set; }

        public byte SportType { get; set; }

        public byte FailedActionId { get; set; }

        public bool DisabledAutoSettle { get; set; }
    }
}