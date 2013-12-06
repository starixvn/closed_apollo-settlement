// -----------------------------------------------------------------------
// <copyright file="MatchStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DTO.Domain
{
    /// <summary>
    ///    Status for match.
    /// </summary>
    public static class MatchStatus
    {
        /// <summary>
        /// Status all.
        /// </summary>
        public const int All = 0;

        /// <summary>
        /// Status running.
        /// </summary>
        public const int Running = 1;

        /// <summary>
        /// Status close.
        /// </summary>
        public const int Closed = 2;

        /// <summary>
        /// Status complete.
        /// </summary>
        public const int Completed = 3;

        /// <summary>
        /// Status refund.
        /// </summary>
        public const int Refund = 4;
    }
}