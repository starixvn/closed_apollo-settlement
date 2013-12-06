// -----------------------------------------------------------------------
// <copyright file="Utility.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Formater Functions
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Web.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.Domain;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class Utility
    {
        #region Map event status to coresponding language

        /// <summary>
        /// Action Style.
        /// </summary>
        public static readonly Dictionary<byte, string> RefActionStyle = new Dictionary<byte, string>
        {
           { 1, "calc-notifier act-calc-ref|FG/NG" },
           { 2, "calc-notifier act-calc-ref|HT" },
           { 3, "calc-notifier act-calc-ref|FT" },
           { 4, "calc-notifier act-calc-ref|All" },
           { 5, "calc-notifier act-parlay-ref|Parlay" },
           { 6, "calc-notifier act-recalc|Re-Calc" },
           { 11, "calc-notifier rf|HT" },
           { 12, "calc-notifier rf|FT" },
           { 13, "calc-notifier rf|All" },
           { 14, "calc-notifier rf|Parlay HT" },
           { 15, "calc-notifier rf|Parlay FT" },
           { 16, "calc-notifier rf|Parlay All" },
           { 17, "calc-notifier rf|All FGLG" },
           { 18, "calc-notifier rf|All LG" },
           { 0, "calc-notifier unsettle act-calc-ref|Uncalculate" }
        };

        /// <summary>
        /// Constant Post.
        /// </summary>
        private const string PostPoned = "postponed";

        /// <summary>
        /// Constant Running.
        /// </summary>
        private const string Running = "running";

        /// <summary>
        /// Constant Closed.
        /// </summary>
        private const string Closed = "closed";

        /// <summary>
        /// Define list key for event status.
        /// </summary>
        private static readonly Dictionary<string, int> Event = new Dictionary<string, int>
            {
                { "running", Convert.ToInt32("0000000", 2) },
                { "closed", Convert.ToInt32("0000010", 2) }
            };

        /// <summary>
        /// Define list key for Result: isPending(1 bit) isHTCal(1 bit) calculate(1 bit) event Status(4 bit).
        /// </summary>
        private static readonly Dictionary<int, string> Result = new Dictionary<int, string>
            {
                { Convert.ToInt32("1000010", 2), "pending" },
                { Convert.ToInt32("1100010", 2), "pending" },
                { Convert.ToInt32("1010010", 2), "pending" },
                { Convert.ToInt32("1000000", 2), "pending" }
            };

        /// <summary>
        ///     Map event status.
        /// </summary>
        /// <param name="eventstatus">Event Status.</param>
        /// <param name="pendingValue">Is Pending.</param>
        /// <param name="halfTime">Is HT-calculation.</param>
        /// <param name="calculate">Calculation parameter.</param>
        /// <returns>Return status.</returns>
        public static string MapEventStatusString(string eventstatus, bool pendingValue, bool halfTime, bool calculate)
        {
            if (string.IsNullOrEmpty(eventstatus))
            {
                return UppercaseFirst(Running);
            }

            var pending = Convert.ToInt32(pendingValue);
            var halfTimeCal = Convert.ToInt32(halfTime);
            var cal = Convert.ToInt32(calculate);

            pending <<= 6; // Shift right 6 bit for pending status
            halfTimeCal <<= 5; // Shift right 5 bit for haft time calculate status
            cal <<= 4; // Shift right 4 bit for calculate status

            var resultKey = pending | halfTimeCal | cal;

            if (Event.ContainsKey(eventstatus.ToLowerInvariant()))
            {
                resultKey |= Event[eventstatus.ToLowerInvariant()]; // Or with event status
                return UppercaseFirst(Result.ContainsKey(resultKey) ? Result[resultKey] : eventstatus);
            }

            return UppercaseFirst(eventstatus);
        }

        #endregion Map event status to coresponding language

        /// <summary>
        ///     Return Score value.
        /// </summary>
        /// <param name="value">Score value.</param>
        /// <param name="hasEmpty">Empty value.</param>
        /// <returns>Return score.</returns>
        public static string GetScore(object value, bool hasEmpty = false)
        {
            if (value != null && value != DBNull.Value)
            {
                return Convert.ToString(value);
            }
            else
            {
                return hasEmpty ? string.Empty : "-";
            }
        }

        /// <summary>
        ///     Return nickname Confirm Halftime and Fulltime.
        /// </summary>
        /// <param name="value">Nick name.</param>
        /// <returns>Return nick name.</returns>
        public static string GetNickNameConfirm(string value)
        {
            return string.IsNullOrEmpty(value) ? "-" : value;
        }

        /// <summary>
        ///     Set background column.
        /// </summary>
        /// <param name="value">Status value.</param>
        /// <param name="backgroundColor">Background value.</param>
        /// <returns>Return background.</returns>
        public static string BackgroundColor(object value, string backgroundColor)
        {
            return Convert.ToBoolean(value) ? backgroundColor : string.Empty;
        }

        /// <summary>
        ///     Set background event status column.
        /// </summary>
        /// <param name="status">Event status.</param>
        /// <param name="pending">Status pending.</param>
        /// <returns>Return background.</returns>
        public static string BackgroundColorEventStatus(string status, object pending)
        {
            return status.ToLower() == PostPoned ? Background.Postponed : BackgroundColor(pending, Background.Pending);
        }

        /// <summary>
        ///     Set background Calculate column.
        /// </summary>
        /// <param name="halftimeCalculated">Half time calculate.</param>
        /// <param name="refundCalculated">Refund calculate.</param>
        /// <param name="calculated">Is calculate.</param>
        /// <returns>Return background.</returns>
        public static string BackgroundColorCalculate(object halftimeCalculated, object refundCalculated, object calculated)
        {
            var background = string.Empty;

            if (Convert.ToBoolean(halftimeCalculated))
            {
                // Bill already
                if (Convert.ToBoolean(calculated))
                {
                    background = Background.Calculated;
                }
                else if (Convert.ToBoolean(refundCalculated))
                {
                    // Refund already
                    background = Background.Refund;
                }
                else
                {
                    background = Background.Halftime;
                }
            }
            else
            {
                // Bill already
                if (Convert.ToBoolean(calculated))
                {
                    background = Background.Calculated;
                }
                else if (Convert.ToBoolean(refundCalculated))
                {
                    // Refund already
                    background = Background.Refund;
                }
            }

            return background;
        }

        /// <summary>
        /// Check Now Date.
        /// </summary>
        /// <param name="dt">Date time value.</param>
        /// <returns>Return boolean.</returns>
        public static bool IsNowDate(DateTime dt)
        {
            return dt.Day == DateTime.Now.Day && dt.Month == DateTime.Now.Month && dt.Year == DateTime.Now.Year;
        }

        /// <summary>
        /// Resolve server Url.
        /// </summary>
        /// <param name="serverUrl">Server Url.</param>
        /// <param name="forceHttps">Force https.</param>
        /// <returns>Return url.</returns>
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
            {
                return serverUrl;
            }

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

        /// <summary>
        /// Uppercase first.
        /// </summary>
        /// <param name="s">String value.</param>
        /// <returns>Return string.</returns>
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            var a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Get Action.
        /// </summary>
        /// <param name="faildActionId">Action Id.</param>
        /// <returns>Return action value.</returns>
        public static string GetActionStatusById(byte? faildActionId)
        {
            string action = string.Empty;
            if (!faildActionId.HasValue)
            {
                return action;
            }

            action = GetAction(faildActionId);
            action = action + " Failed";
            return action;
        }

        /// <summary>
        /// Get Action.
        /// </summary>
        /// <param name="faildActionId">Action Id.</param>
        /// <returns>Return action value.</returns>
        private static string GetAction(byte? faildActionId)
        {
            switch (faildActionId)
            {
                case ConfirmedAction.UnCal:
                    return ViewResources.Uncalculate;
                case ConfirmedAction.CalFGNG:
                    return ViewResources.CalculateFGAndNG;
                case ConfirmedAction.CalHT:
                    return ViewResources.CalculateHT;
                case ConfirmedAction.CalFT:
                    return ViewResources.CalculateFT;
                case ConfirmedAction.CalAll:
                    return ViewResources.CalculateAll;
                case ConfirmedAction.CalMixParlay:
                    return ViewResources.CalcMixParlay;
                case ConfirmedAction.RefundFT:
                    return ViewResources.RefundFT;
                case ConfirmedAction.RefundHT:
                    return ViewResources.RefundHT;
                case ConfirmedAction.RefundAll:
                    return ViewResources.RefundAll;
                case ConfirmedAction.RefundParlayHT:
                    return ViewResources.RefundMultiHT;
                case ConfirmedAction.RefundParlayFT:
                    return ViewResources.RefundMultiFT;
                case ConfirmedAction.RefundParlayAll:
                    return ViewResources.RefundMultiAll;
                case ConfirmedAction.RefundAllFGLG:
                    return ViewResources.RefundAllFGLG;
                case ConfirmedAction.RefundAllLG:
                    return ViewResources.RefundAllLastGoal;
                case ConfirmedAction.UpdateEventStatus:
                    return ViewResources.UpdateEventStatus;
                case ConfirmedAction.UpdateTicket:
                    return ViewResources.UpdateTicket;
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Class Background.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
              "Microsoft.StyleCop.CSharp.MaintainabilityRules",
              "SA1402:FileMayOnlyContainASingleClass",
              Justification = "Splitting this file into classes would get too confusing.")]
    public static class Background
    {
        /// <summary>
        /// Constant Post.
        /// </summary>
        public const string Postponed = "bg-postponed";

        /// <summary>
        /// Constant Pending.
        /// </summary>
        public const string Pending = "bg-pending";

        /// <summary>
        /// Constant Calculated.
        /// </summary>
        public const string Calculated = "bg-calculated";

        /// <summary>
        /// Constant Refund.
        /// </summary>
        public const string Refund = "bg-refund";

        /// <summary>
        /// Constant Halftime.
        /// </summary>
        public const string Halftime = "bg-halftime";

        /// <summary>
        /// Constant Undecided.
        /// </summary>
        public const string Undecided = "bg-row-undecided";

        /// <summary>
        /// Constant IconYes.
        /// </summary>
        public const string IconYes = "icon-yes";

        /// <summary>
        /// Constant IconNoDisable.
        /// </summary>
        public const string IconNoDisable = "icon-no-disable";

        /// <summary>
        /// Constant IconNo.
        /// </summary>
        public const string IconNo = "icon-no";

        /// <summary>
        /// Constant ActionFailed.
        /// </summary>
        public const string ActionFailed = "bg-row-actionfailed";
    }
}