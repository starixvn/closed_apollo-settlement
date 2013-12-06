// -----------------------------------------------------------------------
// <copyright file="IAutoSettlementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.24">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Services.Interface
{
    using Fanex.NAP.Settlement.DTO.DBResult;

    /// <summary>
    /// IAuto Settlement Service.
    /// </summary>
    public interface IAutoSettlementService
    {
        /// <summary>
        /// Update StatusAutoSettlement.
        /// </summary>
        /// <param name="creator">Creator by.</param>
        /// <param name="matchId">Match Id parameter.</param>
        /// <param name="turnOn">Turn on parameter.</param>
        /// <param name="ip">IP parameter.</param>
        /// <returns>Return result.</returns>
        IResult UpdateStatusAutoSettlement(int creator, int matchId, bool turnOn, string ip);

        /// <summary>
        /// Status AutoSettlement.
        /// </summary>
        /// <returns>Return boolean.</returns>
        bool GetStatusAutoSettlement();

        /// <summary>
        /// Clear fail action AutoSettlement.
        /// </summary>
        /// <param name="creator">Creator by.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <param name="matchIds">List match ids.</param>
        /// <returns>Return result.</returns>
        IResult ClearFailActionAutoSettlement(int creator, int matchId, string matchIds);
    }
}