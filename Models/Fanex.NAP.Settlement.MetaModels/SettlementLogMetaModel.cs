// -----------------------------------------------------------------------
// <copyright file="SettlementLogMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.MetaModels
{
    using System;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SettlementLogMetaModel
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="SettlementLogMetaModel" /> class.
        /// </summary>
        public SettlementLogMetaModel()
        {
            this.FromDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());
            this.ToDate = DateTime.Today;
        }

        /// <summary>
        /// Gets or sets MatchCode.
        /// </summary>
        /// <value>The match code of settlement log.</value>
        public string MatchCode { get; set; }

        /// <summary>
        /// Gets or sets FromDate.
        /// </summary>
        /// <value>The from day of settlement log.</value>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets ToDate.
        /// </summary>
        /// <value>The to day of settlement log.</value>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets or sets BalanceUpDay.
        /// </summary>
        /// <value>The balance up day of settlement log.</value>
        public DateTime BalanceUpDay { get; set; }
    }
}