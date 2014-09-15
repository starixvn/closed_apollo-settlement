// -----------------------------------------------------------------------
// <copyright file="CalculateAction.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Core.Constants
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CalculateAction
    {
        /// <summary>
        /// Calculate All.
        /// </summary>
        public const string CalculateAll = "CAL";

        /// <summary>
        /// Calculate HalfTime.
        /// </summary>
        public const string CalculateHalfTime = "CAL-HT";

        /// <summary>
        /// Calculate FullTime.
        /// </summary>
        public const string CalculateFullTime = "CAL-FT";

        /// <summary>
        /// Constant UnCalculate.
        /// </summary>
        public const string Uncalculate = "UNDO";

        /// <summary>
        /// Constant Refund.
        /// </summary>
        public const string Refund = "REFUND";

        /// <summary>
        /// Constant RefundHalfTime.
        /// </summary>
        public const string RefundHalfTime = "REFUND-HT";

        /// <summary>
        /// Constant RefundFullTime.
        /// </summary>
        public const string RefundFullTime = "REFUND-FT";

        /// <summary>
        /// Refund All FGNG.
        /// </summary>
        public const string RefundAllFGNG = "REFUND-FNG";

        /// <summary>
        /// Refund All LastGoal.
        /// </summary>
        public const string RefundAllLastGoal = "REFUND-ALG";
    }
}