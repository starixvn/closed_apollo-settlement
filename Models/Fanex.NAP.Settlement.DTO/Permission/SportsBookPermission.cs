// -----------------------------------------------------------------------
// <copyright file="SportsBookPermission.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  SportsBookPermission Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DTO.Permission
{
    /// <summary>
    /// Sports Book Permission.
    /// </summary>
    public class SportsBookPermission
    {
        public bool CanCalculate { get; set; }

        public bool CanUncalculate { get; set; }

        public bool CanRefund { get; set; }

        public bool CanCalculateMp { get; set; }
    }
}