// -----------------------------------------------------------------------
// <copyright file="IOutrightService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.16">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    /// Interface Outright Service.
    /// </summary>
    public interface IOutrightService
    {
        /// <summary>
        /// Get list outright settlement.
        /// </summary>
        /// <param name="request">Request parameter outright.</param>
        /// <param name="actionLog">Out list action log.</param>
        /// <returns>Return outright list.</returns>
        IList<Outright> GetOutrightList(OutrightMetaModel request, out IList<OutrightLogAction> actionLog);

        /// <summary>
        /// Get all matches under this league.
        /// </summary>
        /// <param name="leagueId">League id.</param>
        /// <param name="isSettled">Match is settled.</param>
        /// <param name="actionLog">Action log.</param>
        /// <returns>Return outright list.</returns>
        IList<Outright> GetMatchesByLeagueId(int leagueId, bool isSettled, out IList<OutrightLogAction> actionLog);

        /// <summary>
        /// Get log action outright.
        /// </summary>
        /// <param name="outrightId">Outright Id.</param>
        /// <returns>Return outright log action list.</returns>
        IList<OutrightLogAction> GetLogAction(int outrightId);

        /// <summary>
        /// Update Outright Event.
        /// </summary>
        /// <param name="request">Outright parameter.</param>
        /// <returns>Return result.</returns>
        IResult OutrightEvent(OutrightCalculateMetaModel request);

        /// <summary>
        /// Update Outright Trans.
        /// </summary>
        /// <param name="request">Outright parameter.</param>
        /// <returns>Return result.</returns>
        IResult OutrightTrans(OutrightCalculateMetaModel request);

        /// <summary>
        /// Update Flag Outright.
        /// </summary>
        /// <param name="request">Outright auto parameter.</param>
        /// <returns>Return result.</returns>
        IResult UpdateFlagOutright(OutrightAutoSettleMetaModel request);

        /// <summary>
        /// Clear Fail Action Outright.
        /// </summary>
        /// <param name="outrightId">Outright Id.</param>
        /// <returns>Return result.</returns>
        IResult ClearFailActionOutright(int outrightId);
    }
}