// -----------------------------------------------------------------------
// <copyright file="ConfirmedAction.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.Core.Constants
{
    /// <summary>
    /// Confirmed Action.
    /// </summary>
    public static class ConfirmedAction
    {
        /// <summary>
        /// Un Calculate.
        /// </summary>
        public const byte UnCal = 0;

        /// <summary>
        /// Calculate FG/NG.
        /// </summary>
        public const byte CalFGNG = 1;

        /// <summary>
        /// Calculate Half time.
        /// </summary>
        public const byte CalHT = 2;

        /// <summary>
        /// Calculate full time.
        /// </summary>
        public const byte CalFT = 3;

        /// <summary>
        /// Calculate all.
        /// </summary>
        public const byte CalAll = 4;

        /// <summary>
        /// Calculate Mix Parlay.
        /// </summary>
        public const byte CalMixParlay = 5;

        /// <summary>
        /// Re-Calculate for NumberGame.
        /// </summary>
        public const byte ReCalculate = 6;

        /// <summary>
        /// Refund half time.
        /// </summary>
        public const byte RefundHT = 11;

        /// <summary>
        /// Refund full time.
        /// </summary>
        public const byte RefundFT = 12;

        /// <summary>
        /// Refund all.
        /// </summary>
        public const byte RefundAll = 13;

        /// <summary>
        /// Refund parlay half time.
        /// </summary>
        public const byte RefundParlayHT = 14;

        /// <summary>
        /// Refund parlay full time.
        /// </summary>
        public const byte RefundParlayFT = 15;

        /// <summary>
        /// Refund parlay all.
        /// </summary>
        public const byte RefundParlayAll = 16;

        /// <summary>
        /// Refund all FG/NG.
        /// </summary>
        public const byte RefundAllFGLG = 17;

        /// <summary>
        /// Refund all LG.
        /// </summary>
        public const byte RefundAllLG = 18;

        /// <summary>
        /// Update event status.
        /// </summary>
        public const byte UpdateEventStatus = 30;

        /// <summary>
        /// Update ticket.
        /// </summary>
        public const byte UpdateTicket = 31;
    }
}