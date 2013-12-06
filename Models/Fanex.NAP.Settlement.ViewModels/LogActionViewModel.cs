// -----------------------------------------------------------------------
// <copyright file="LogActionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Log Action ViewModel Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.11.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.ViewModels
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class LogActionViewModel
    {
        /// <summary>
        ///     Gets or sets list action log.
        /// </summary>
        /// <value>The list match log of the log action view.</value>
        public IList<MatchLog> MatchLogList { get; set; }

        /// <summary>
        ///     Gets or sets Match object.
        /// </summary>
        /// <value>The match detail of the log action view.</value>
        public MatchDetail CurrentMatch { get; set; }

        /// <summary>
        /// Gets or sets Template.
        /// </summary>
        /// <value>The template of the log action view.</value>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets Match group.
        /// </summary>
        /// <value>The group sport of the log action view.</value>
        public int GroupSport { get; set; }
    }
}