// --------------------------------------------------------------------------
// <copyright file="OutrightMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.17">Created</change>
// </history>

namespace Fanex.NAP.Settlement.MetaModels
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;

    /// <summary>
    /// OutrightMetaModel class.
    /// </summary>
    public class OutrightMetaModel
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="OutrightMetaModel" /> class.
        /// </summary>
        public OutrightMetaModel()
        {
            this.FromDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());
            this.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            this.SportTypeId = 1;
        }

        /// <summary>
        ///     Gets or sets sportId.
        /// </summary>
        /// <value>The sport id of outright.</value>
        public int SportTypeId { get; set; }

        /// <summary>
        ///     Gets or sets LeagueId.
        /// </summary>
        /// <value>The league id of outright.</value>
        public int LeagueId { get; set; }

        /// <summary>
        ///     Gets or sets sports list.
        /// </summary>
        /// <value>The list sport of outright.</value>
        public IList<Sport> SportsList { get; set; }

        /// <summary>
        ///     Gets or sets League list.
        /// </summary>
        /// <value>The list league of outright.</value>
        public IList<League> LeaguesList { get; set; }

        /// <summary>
        ///     Gets or sets From date.
        /// </summary>
        /// <value>The from date of outright.</value>
        public DateTime FromDate { get; set; }

        /// <summary>
        ///     Gets or sets To date.
        /// </summary>
        /// <value>The to date of outright.</value>
        public DateTime ToDate { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is Settled.
        /// </summary>
        /// <value>The IsSettled of outright.</value>
        public bool IsSettled { get; set; }
    }
}