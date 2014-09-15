// -----------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Base ViewModel Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.09.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.ViewModels
{
    using Fanex.NAP.Settlement.Core.Constants;
    using Fanex.NAP.Settlement.DTO.Permission;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BaseViewModel
    {
        private readonly string _leagueName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        public BaseViewModel()
        {
            this.Permission = new SettlementPermission();
        }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_leagueName) ? CacheRef.GetLeagueName(LeagueId) : _leagueName;
            }
        }

        public int GroupSport { get; set; }

        public string Template { get; set; }

        public int Colspan { get; set; }

        public string HeaderEvent { get; set; }

        public PageType PageType { get; set; }

        public SettlementPermission Permission { get; set; }

        public bool DisabledAutoSettle { get; set; }
    }
}