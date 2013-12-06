// -----------------------------------------------------------------------
// <copyright file="IAutoSettlementDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.24">Created</change>
// </history>
namespace Fanex.NAP.Settlement.DataAccess.Interface
{
    using Fanex.NAP.Settlement.DTO.DBResult;

    /// <summary>
    /// Interface Auto Settlement DataAccess.
    /// </summary>
    public interface IAutoSettlementDataAccess
    {
        /// <summary>
        /// Update status AutoSettlement.
        /// </summary>
        /// <param name="creator">Creator by.</param>
        /// <param name="matchId">Match Id parameter.</param>
        /// <param name="turnOn">Turn on parameter.</param>
        /// <param name="ip">Internet protocol.</param>
        /// <returns>Return result.</returns>
        IResult UpdateStatusAutoSettlement(int creator, int matchId, bool turnOn, string ip);

        /// <summary>
        /// Get status AutoSettlement.
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