// -----------------------------------------------------------------------
// <copyright file="LeagueService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Leagues Service Implement
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.17">Created and Implement function get Leagues</change>
// </history>

namespace Fanex.NAP.Settlement.Services.Implement
{
    #region

    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DataAccess.Implement;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.Services.Interface;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class LeagueService : ILeagueService
    {
        #region Fields

        /// <summary>
        /// Leagues DataAccess.
        /// </summary>
        private readonly LeagueDataAccess _leaguesDataAccess;

        #endregion Fields

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="LeagueService" /> class.
        /// </summary>
        public LeagueService()
        {
            this._leaguesDataAccess = new LeagueDataAccess();
        }

        #endregion Constructor

        /// <summary>
        /// Get list league.
        /// </summary>
        /// <param name="param">Parameter request.</param>
        /// <returns>Return league list.</returns>
        public IList<League> GetLeagueList(LeagueMetaModel param)
        {
            return param.IsValid
                       ? this._leaguesDataAccess.GetLeagueList(param)
                       : new List<League>();
        }
    }
}