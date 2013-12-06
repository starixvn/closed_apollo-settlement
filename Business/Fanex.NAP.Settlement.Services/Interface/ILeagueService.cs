// -----------------------------------------------------------------------
// <copyright file="ILeagueService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Services.Interface
{
    #region

    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.MetaModels;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ILeagueService
    {
        /// <summary>
        /// Get list league.
        /// </summary>
        /// <param name="param">Parameter request.</param>
        /// <returns>Return league list.</returns>
        IList<League> GetLeagueList(LeagueMetaModel param);
    }
}