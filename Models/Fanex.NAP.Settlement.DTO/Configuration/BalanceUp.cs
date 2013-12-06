// -----------------------------------------------------------------------
// <copyright file="BalanceUp.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  DTO BalanceUp
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.14">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.DTO.Configuration
{
    #region

    using System;

    #endregion

    /// <summary>
    /// Balance Up.
    /// </summary>
    public class BalanceUp
    {
        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
    }
}