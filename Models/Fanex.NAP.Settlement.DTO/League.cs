// -----------------------------------------------------------------------
// <copyright file="League.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  DTO League
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.DTO
{
    using System;

    /// <summary>
    /// Class League.
    /// </summary>
    [Serializable]
    public class League
    {
        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public byte SportType { get; set; }
    }
}