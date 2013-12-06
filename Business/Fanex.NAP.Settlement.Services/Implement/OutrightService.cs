// -----------------------------------------------------------------------
// <copyright file="OutrightService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.16">Created</change>
// </history>
namespace Fanex.NAP.Settlement.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DataAccess.Implement;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.Services.Interface;

    /// <summary>
    /// Outright service.
    /// </summary>
    public class OutrightService : IOutrightService
    {
        #region Fields

        /// <summary>
        /// Outright DataAccess.
        /// </summary>
        private readonly OutrightDataAccess _outrightDataAccess;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OutrightService" /> class.
        /// </summary>
        public OutrightService()
        {
            this._outrightDataAccess = new OutrightDataAccess();
        }

        #endregion Constructor

        /// <summary>
        /// Get list outright settlement.
        /// </summary>
        /// <param name="request">Request parameter outright.</param>
        /// <param name="actionLog">Out list action log.</param>
        /// <returns>Return outright list.</returns>
        public IList<Outright> GetOutrightList(OutrightMetaModel request, out IList<OutrightLogAction> actionLog)
        {
            return this._outrightDataAccess.GetOutrightList(request, out actionLog);
        }

        /// <summary>
        /// Get all matches under this league.
        /// </summary>
        /// <param name="leagueId">League id.</param>
        /// <param name="isSettled">Match is settled.</param>
        /// <param name="actionLog">Action log.</param>
        /// <returns>Return outright list.</returns>
        public IList<Outright> GetMatchesByLeagueId(int leagueId, bool isSettled, out IList<OutrightLogAction> actionLog)
        {
            return this._outrightDataAccess.GetMatchesByLeagueId(leagueId, isSettled, out actionLog);
        }

        /// <summary>
        /// Get log action outright.
        /// </summary>
        /// <param name="outrightId">Outright Id.</param>
        /// <returns>Return outright log action list.</returns>
        public IList<OutrightLogAction> GetLogAction(int outrightId)
        {
            return this._outrightDataAccess.GetLogAction(outrightId);
        }

        /// <summary>
        /// Update Outright Event.
        /// </summary>
        /// <param name="request">Outright parameter.</param>
        /// <returns>Return result.</returns>
        public IResult OutrightEvent(OutrightCalculateMetaModel request)
        {
            return this._outrightDataAccess.OutrightEvent(request);
        }

        /// <summary>
        /// Update Outright Trans.
        /// </summary>
        /// <param name="request">Outright parameter.</param>
        /// <returns>Return result.</returns>
        public IResult OutrightTrans(OutrightCalculateMetaModel request)
        {
            return this._outrightDataAccess.OutrightTrans(request);
        }

        /// <summary>
        /// Update Flag Outright.
        /// </summary>
        /// <param name="request">Outright auto parameter.</param>
        /// <returns>Return result.</returns>
        public IResult UpdateFlagOutright(OutrightAutoSettleMetaModel request)
        {
            return this._outrightDataAccess.UpdateFlagOutright(request);
        }

        /// <summary>
        /// Clear Fail Action Outright.
        /// </summary>
        /// <param name="outrightId">Outright Id.</param>
        /// <returns>Return result.</returns>
        public IResult ClearFailActionOutright(int outrightId)
        {
            return this._outrightDataAccess.ClearFailActionOutright(outrightId);
        }
    }
}