// -----------------------------------------------------------------------
// <copyright file="ILeagueDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DataAccess.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ILeagueDataAccess
    {
        /// <summary>
        /// Get list league.
        /// </summary>
        /// <param name="param">Parameter request.</param>
        /// <returns>Return league list.</returns>
        IList<League> GetLeagueList(LeagueMetaModel param);
    }
}