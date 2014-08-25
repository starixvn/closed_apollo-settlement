// -----------------------------------------------------------------------
// <copyright file="SportViewSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Auto get template content and header
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.14">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Core.Configuration
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SportViewSetting
    {
        public int SportType { get; set; }

        public int GroupSport { get; set; }

        public string Template { get; set; }

        public string HeaderGroup { get; set; }

        public string ContentGroup { get; set; }

        public string HeaderSportType { get; set; }

        public string ContentSportType { get; set; }

        public string DirectlyTemplate { get; set; }

        public bool IsCheckBoxCalcMp { get; set; }

        public int Colspan { get; set; }
    }
}