// -----------------------------------------------------------------------
// <copyright file="OutrightLogActionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculation ViewModel Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.18">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.ViewModels
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.Domain;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class OutrightLogActionViewModel : BaseViewModel
    {
        private readonly string _teamName = string.Empty;

        /// <summary>
        ///     Gets or sets Team Id.
        /// </summary>
        /// <value>The team id of the view.</value>
        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_teamName) ? CacheRef.GetTeamName(TeamId) : _teamName;
            }
        }

        /// <summary>
        ///     Gets or sets Outright list.
        /// </summary>
        /// <value>The list outright of the view.</value>
        public IList<Outright> OutrightList { get; set; }

        /// <summary>
        ///     Gets or sets parameter.
        /// </summary>
        /// <value>The list outright log action of the view.</value>
        public IList<OutrightLogAction> LogActionList { get; set; }
    }
}