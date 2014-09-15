// -----------------------------------------------------------------------
// <copyright file="OutrightAutoSettleMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
// Outright Auto SettleMetaModel
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.21">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.MetaModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Outright Auto SettleMetaModel.
    /// </summary>
    public class OutrightAutoSettleMetaModel
    {
        /// <summary>
        ///     Gets or sets AdminId.
        /// </summary>
        /// <value>The AdminId of outright auto.</value>
        public int AdminId { get; set; }

        /// <summary>
        ///     Gets or sets LeagueId.
        /// </summary>
        /// <value>The league id of outright auto.</value>
        public int LeagueId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is TurnOn.
        /// </summary>
        /// <value>The turn On of outright auto.</value>
        public bool IsTurnOn { get; set; }

        /// <summary>
        ///     Gets or sets IP.
        /// </summary>
        /// <value>The IP of outright auto.</value>
        public string IP { get; set; }
    }
}