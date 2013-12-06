// -----------------------------------------------------------------------
// <copyright file="LeagueMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  MetaModel League
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.21">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.MetaModels
{
    using System;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class LeagueMetaModel
    {
        /// <summary>
        /// Gets or sets FromDate.
        /// </summary>
        /// <value>The from date of the league.</value>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets ToDate.
        /// </summary>
        /// <value>The to date of the league.</value>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets or sets SportTypeId.
        /// </summary>
        /// <value>The sport type of the league.</value>
        public int SportTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is Outright.
        /// </summary>
        /// <value>The outright of the league.</value>
        public bool IsOutright { get; set; }

        /// <summary>
        /// Gets a value indicating whether the item is Valid.
        /// </summary>
        /// <value>The valid of the league.</value>
        public bool IsValid
        {
            get { return this.FromDate <= this.ToDate; }
        }
    }
}