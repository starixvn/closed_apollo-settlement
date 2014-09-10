// -----------------------------------------------------------------------
// <copyright file="MatchLog.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  MatchLog Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.DTO
{
    using System;
    using Fanex.NAP.Settlement.Core.Constants;

    /// <summary>
    /// Class MatchLog.
    /// </summary>
    public class MatchLog
    {
        private string _toolTip;

        private BetStatusAccess _betStatusAccess;

        public long LogId { get; set; }

        public int MatchId { get; set; }

        public byte ActionId { get; set; }

        public byte SportType { get; set; }

        public int GroupSport { get; set; }

        public string BetStatus { get; set; }

        public string GoalRecord { get; set; }

        public DateTime LogTime { get; set; }

        public bool IsSettled { get; set; }

        public bool Disabled { get; set; }

        #region Match Score

        public int? HtHomeScore { get; set; }

        public int? HtAwayScore { get; set; }

        public int? FinalHomeScore { get; set; }

        public int? FinalAwayScore { get; set; }

        public bool IsExistHTFGLG { get; set; }

        public bool IsExistFGLG { get; set; }

        #endregion Match Score

        #region Extra Property

        public BetStatusAccess BetStatusAccess
        {
            get
            {
                if (!Convert.IsDBNull(BetStatus) && !string.IsNullOrEmpty(BetStatus) && this.ActionId < 11)
                {
                    _betStatusAccess = new BetStatusAccess(GroupSport, BetStatus, IsExistFGLG, IsExistHTFGLG, GoalRecord, false);
                }
                else if (!string.IsNullOrWhiteSpace(GoalRecord) && this.ActionId < 11)
                {
                    var isFinalScore = FinalAwayScore != null && FinalHomeScore != null;

                    _betStatusAccess = new BetStatusAccess(GroupSport, BetStatus, IsExistFGLG, IsExistHTFGLG, GoalRecord, isFinalScore);
                }
                else
                {
                    _betStatusAccess = new BetStatusAccess();
                }

                return this._betStatusAccess;
            }
        }

        #endregion Extra Property

        #region Tooltip

        public string Tooltip
        {
            get
            {
                switch (this.GroupSport)
                {
                    // Basketball, Football, Rugby, Netball
                    case 5:
                    case 2:
                        this._toolTip = this.BasketBall();
                        break;

                    // IceHockey, FiledHockey
                    case 3:
                        this._toolTip = this.Hockey();
                        break;

                    // Tennis, Volleyball, Badminton, Squash
                    case 4:
                    case 7:
                        this._toolTip = this.Tennis();
                        break;

                    // Billiard
                    case 6:
                        this._toolTip = this.Billiard();
                        break;

                    // Other Sports
                    default:
                        this._toolTip = this.Soccer();
                        break;
                }

                return this._toolTip;
            }
        }

        /// <summary>
        /// SportType Soccer.
        /// </summary>
        /// <returns>Return score.</returns>
        public string Soccer()
        {
            this.GoalRecord = string.IsNullOrWhiteSpace(this.GoalRecord) ? string.Empty : this.GoalRecord;

            switch (this.ActionId)
            {
                case ConfirmedAction.CalHT:
                case ConfirmedAction.CalFT:
                case ConfirmedAction.CalAll:
                case ConfirmedAction.CalFGNG:
                case ConfirmedAction.CalculateHTFGNG:
                    var halfTimeScore = string.Join("-", this.HtHomeScore, this.HtAwayScore);
                    var finalScore = string.Join("-", this.FinalHomeScore, this.FinalAwayScore);
                    if (!string.IsNullOrWhiteSpace(halfTimeScore) && !string.IsNullOrWhiteSpace(finalScore))
                    {
                        return string.Join(" ; ", halfTimeScore, finalScore, this.GoalRecord.ToUpper());
                    }
                    else if (!string.IsNullOrWhiteSpace(halfTimeScore))
                    {
                        return string.Join(" ; ", halfTimeScore, this.GoalRecord.ToUpper());
                    }
                    else if (!string.IsNullOrWhiteSpace(finalScore))
                    {
                        return string.Join(" ; ", halfTimeScore, this.GoalRecord.ToUpper());
                    }
                    else
                        return this.GoalRecord.ToUpper();

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// SportType Billiard.
        /// </summary>
        /// <returns>Return score.</returns>
        public string Billiard()
        {
            if (this.ActionId == ConfirmedAction.CalAll)
            {
                if (string.IsNullOrWhiteSpace(this.BetStatus))
                {
                    return string.Join("-", this.FinalHomeScore, this.FinalAwayScore);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Sport Type.
        /// </summary>
        /// <returns>Return score.</returns>
        private string Tennis()
        {
            if (this.ActionId == ConfirmedAction.CalAll)
            {
                if (string.IsNullOrWhiteSpace(this.BetStatus))
                {
                    return string.Join("-", this.FinalHomeScore, this.FinalAwayScore);
                }

                var betStatus = this.BetStatus.Replace(",", " , ");
                var home = this.BetStatusAccess.HomeTotalGames;
                var away = this.BetStatusAccess.AwayTotalGames;
                return string.Format("{0} ; {1}-{2} ; {3}-{4}", betStatus, home, away, this.FinalHomeScore, this.FinalAwayScore);
            }

            return string.Empty;
        }

        /// <summary>
        /// Sport Type Basketball.
        /// </summary>
        /// <returns>Return score.</returns>
        private string BasketBall()
        {
            string result = string.Empty;

            switch (this.ActionId)
            {
                case ConfirmedAction.CalHT:
                    result = string.Join("-", this.HtHomeScore, this.HtAwayScore);
                    break;

                case ConfirmedAction.CalFT:
                    result = string.Join("-", this.FinalHomeScore, this.FinalAwayScore);
                    break;

                case ConfirmedAction.CalAll:
                    var home = string.Join("-", this.HtHomeScore, this.HtAwayScore);
                    var away = string.Join("-", this.FinalHomeScore, this.FinalAwayScore);
                    result = string.Join(" ; ", home, away);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(this.BetStatus))
            {
                result = string.Join(" ; ", this.BetStatus.Replace(",", " , "), result);
            }

            return result;
        }

        /// <summary>
        /// Sport Type IceHockey.
        /// </summary>
        /// <returns>Return score.</returns>
        private string Hockey()
        {
            if (this.ActionId == ConfirmedAction.CalAll)
            {
                if (string.IsNullOrWhiteSpace(this.BetStatus))
                {
                    return string.Join("-", this.FinalHomeScore, this.FinalAwayScore);
                }

                return string.Format("{0} ; {1}-{2}", this.BetStatus.Replace(",", " , "), this.FinalHomeScore, this.FinalAwayScore);
            }

            return string.Empty;
        }

        #endregion Tooltip
    }
}