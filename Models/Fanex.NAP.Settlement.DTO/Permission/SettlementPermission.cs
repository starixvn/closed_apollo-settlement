// -----------------------------------------------------------------------
// <copyright file="SettlementPermission.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  SettlementPermission Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DTO.Permission
{
    /// <summary>
    /// Settlement Permission.
    /// </summary>
    public class SettlementPermission
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettlementPermission" /> class.
        /// </summary>
        public SettlementPermission()
        {
            this.SportsBook = new SportsBookPermission();
        }

        public bool CanViewSettlementLog { get; set; }

        public bool CanViewSettlementResult { get; set; }

        public SportsBookPermission SportsBook { get; set; }

        public bool CanSetAutoSettlement { get; set; }

        public bool CanCalculateOutright { get; set; }

        public bool CanUncalculateOutright { get; set; }
    }
}