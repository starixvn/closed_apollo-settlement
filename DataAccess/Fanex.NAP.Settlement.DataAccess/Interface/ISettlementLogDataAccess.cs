// -----------------------------------------------------------------------
// <copyright file="ISettlementLogDataAccess.cs" company="Nexcel Solutions Vietnam">
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
    public interface ISettlementLogDataAccess
    {
        /// <summary>
        /// Load MatchInfo.
        /// </summary>
        /// <param name="param">Settlement log parameter.</param>
        /// <returns>Return SettlementLog list.</returns>
        IList<SettlementLog> LoadMatchInfo(SettlementLogMetaModel param);

        /// <summary>
        /// Match log detail.
        /// </summary>
        /// <param name="matchId">Match id parameter.</param>
        /// <returns>Return log list.</returns>
        IList<SettlementLogDetails> LoadMatchInfoDetails(int matchId);
    }
}