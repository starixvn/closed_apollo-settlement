// -----------------------------------------------------------------------
// <copyright file="MatchDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Match Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DTO
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class MatchDetail : Match
    {
        #region Calculation Information

        public bool IsExistFgLg { get; set; }

        public bool IsExistLG { get; set; }

        public bool IsHtOnly { get; set; }

        public bool IsLive { get; set; }

        public int DelayTime { get; set; }

        public int PlayTime { get; set; }

        public int ConfirmFt { get; set; }

        public int ConfirmHt { get; set; }

        #endregion Calculation Information
    }
}