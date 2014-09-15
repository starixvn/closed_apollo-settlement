// -----------------------------------------------------------------------
// <copyright file="CalculationType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Core.Constants
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class CalculationType
    {
        #region Refund ticket

        /// <summary>
        /// Refund Half Time.
        /// </summary>
        public const string RefundHalfTime = "refundhalftime";

        /// <summary>
        /// Refund Full Time.
        /// </summary>
        public const string RefundFullTime = "refundfulltime";

        /// <summary>
        /// Refund first no goal.
        /// </summary>
        public const string RefundAllFgng = "refundfirstnogoal";

        /// <summary>
        /// Action Refund.
        /// </summary>
        public const string RefundAll = "refund";

        /// <summary>
        /// Refund Parlay HalfTime.
        /// </summary>
        public const string RefundParlayHalfTime = "refundparlayhalftime";

        /// <summary>
        /// Refund Parlay FullTime.
        /// </summary>
        public const string RefundParlayFullTime = "refundparlayfulltime";

        /// <summary>
        /// Refund Parlay All.
        /// </summary>
        public const string RefundParlayAll = "refundparlayall";

        #endregion Refund ticket

        #region Calculate ticket

        /// <summary>
        /// Calculate HalfTime.
        /// </summary>
        public const string CalculateHalfTime = "calculatehalftime";

        /// <summary>
        /// Calculate FullTime.
        /// </summary>
        public const string CalculateFullTime = "calculatefulltime";

        /// <summary>
        /// Calculate All.
        /// </summary>
        public const string CalculateAll = "calculateall";

        /// <summary>
        /// Un Calculate.
        /// </summary>
        public const string Uncalculate = "uncalculate";

        /// <summary>
        /// Constant Calculate.
        /// </summary>
        public const string CalculateFgng = "calculatefgng";

        /// <summary>
        /// Calculate Parlay.
        /// </summary>
        public const string CalculateParlay = "calculateparlay";

        public const string RefundHTFirstGoalLastGoal = "refundhalftimefirstgoallastgoal";
        public const string RefundHTLastGoal = "refundhalftimelastgoal";
        public const string RefundFTFirstGoalLastGoal = "refundfulltimefirstgoallastgoal";
        public const string RefundFTLastGoal = "refundfulltimelastgoal";
        public const string RefundAllLastGoal = "refundalllastgoal";

        #endregion Calculate ticket
    }
}