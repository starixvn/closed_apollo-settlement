// -----------------------------------------------------------------------
// <copyright file="EventStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.11.22">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.Core.Constants
{
    /// <summary>
    /// Event status for match.
    /// </summary>
    public class EventStatus
    {
        public const string Completed = "completed";
        public const string Running = "running";
        public const string PostPoned = "postponed";
        public const string RefundHT = "refund ht";
    }
}