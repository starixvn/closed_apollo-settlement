// -----------------------------------------------------------------------
// <copyright file="OutrightLogAction.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Outright Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.18">Created</change>
// </history>
namespace Fanex.NAP.Settlement.DTO
{
    using System;

    /// <summary>
    /// Outright Log Action.
    /// </summary>
    public class OutrightLogAction
    {
        public long LogId { get; set; }

        public int OrId { get; set; }

        public byte ActionId { get; set; }

        public string Status { get; set; }

        public double DeadHeat { get; set; }

        public DateTime LogTime { get; set; }

        public bool IsSettled { get; set; }
    }
}