// -----------------------------------------------------------------------
// <copyright file="CalculateMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculation Request Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.MetaModels
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [Serializable]
    public class CalculateMetaModel
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="CalculateMetaModel" /> class.
        /// </summary>
        public CalculateMetaModel()
        {
            this.FromDate = Convert.ToDateTime(DateTime.Now.AddDays(-30).ToShortDateString()); //Convert.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());
            this.ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            this.SportTypeId = 1;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sport type.
        /// </summary>
        /// <value>The sport type of the calculate.</value>
        public int SportTypeId
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets sports list.
        /// </summary>
        /// <value>The list sports of the calculate.</value>
        public IList<Sport> SportsList { get; set; }

        /// <summary>
        ///     Gets or sets League list.
        /// </summary>
        /// <value>The list leagues of the calculate.</value>
        public IList<League> LeaguesList { get; set; }

        /// <summary>
        ///     Gets or sets From date.
        /// </summary>
        /// <value>The from date of the calculate.</value>
        public DateTime FromDate { get; set; }

        /// <summary>
        ///     Gets or sets To date.
        /// </summary>
        /// <value>The to date of the calculate.</value>
        public DateTime ToDate { get; set; }

        /// <summary>
        ///     Gets or sets LeagueId.
        /// </summary>
        /// <value>The League id of the calculate.</value>
        public int LeagueId { get; set; }

        /// <summary>
        ///     Gets or sets MatchCode.
        /// </summary>
        /// <value>The match code of the calculate.</value>
        public string MatchCode
        {
            get { return "7506"; }// "6110"; }
        }

        //    get;
        //    set;
        //}

        /// <summary>
        ///     Gets or sets StatusId.
        /// </summary>
        /// <value>The status id of the calculate.</value>
        public int StatusId { get; set; }

        /// <summary>
        ///     Gets or sets Filter.
        /// </summary>
        /// <value>The filter by of the calculate.</value>
        public int FilterBy { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is un-calculate.
        /// </summary>
        /// <value>The un-calculate of the calculate.</value>
        public bool UnCalculate { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is Un-CalculateMultiple.
        /// </summary>
        /// <value>The un-calculate multiple of the calculate.</value>
        public bool UncalculateMultiple { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is reload.
        /// </summary>
        /// <value>The re-load of the calculate.</value>
        public bool IsReLoad { get; set; }
    }
}