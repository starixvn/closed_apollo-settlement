// -----------------------------------------------------------------------
// <copyright file="SettlementLogDetails.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  SettlementLogDetails Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DTO
{
    using System;

    /// <summary>
    ///     Match log details.
    /// </summary>
    public class SettlementLogDetails
    {
        private string _remark;
        private string _betStatus;

        public DateTime SettlementDate { get; set; }

        public string UserName { get; set; }

        public string ActionName { get; set; }

        public int? HtHomeScore { get; set; }

        public int? HtAwayScore { get; set; }

        public int? FinalHomeScore { get; set; }

        public int? FinalAwayScore { get; set; }

        public string GoalRecord { get; set; }

        public string BetStatus
        {
            get
            {
                return string.IsNullOrWhiteSpace(this._betStatus)
                    ? "-"
                    : this._betStatus.Replace(",", ", ");
            }

            set
            {
                _betStatus = value;
            }
        }

        public string Remark
        {
            get
            {
                return string.IsNullOrWhiteSpace(this._remark)
                    ? "-"
                    : this._remark;
            }

            set
            {
                _remark = value;
            }
        }

        public string Ip { get; set; }

        public int? BetType { get; set; }
    }
}