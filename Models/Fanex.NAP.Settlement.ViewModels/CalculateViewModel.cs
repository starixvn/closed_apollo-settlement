// -----------------------------------------------------------------------
// <copyright file="CalculateViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculation ViewModel Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created and Implement</change>
//  <change who="Kaden" date="2012.09.14">Rename file and inherit BaseViewModel</change>
// </history>

namespace Fanex.NAP.Settlement.ViewModels
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class CalculateViewModel : BaseViewModel
    {
        /// <summary>
        ///     Gets or sets Match List.
        /// </summary>
        /// <value>The list match of the calculate view.</value>
        public IList<Match> MainMatchList { get; set; }

        /// <summary>
        ///     Gets or sets Log Action List.
        /// </summary>
        /// <value>The list match log of the calculate view.</value>
        public IList<MatchLog> ActionList { get; set; }

        /// <summary>
        ///     Gets or sets sports list.
        /// </summary>
        /// <value>The list sport of the calculate view.</value>
        public IList<Sport> SportsList { get; set; }

        /// <summary>
        ///     Gets or sets League list.
        /// </summary>
        /// <value>The list league of the calculate view.</value>
        public IList<League> LeaguesList { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether checkbox on template.
        /// </summary>
        /// <value>The check box calculate mix parlay of the calculate view.</value>
        public bool IsCheckBoxCalcMp { get; set; }

        /// <summary>
        ///     Gets or sets parameter.
        /// </summary>
        /// <value>The parameter of the calculate view.</value>
        public CalculateMetaModel Param { get; set; }
    }
}