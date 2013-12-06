// -----------------------------------------------------------------------
// <copyright file="SettlementLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Settlement log service
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
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
    public class SettlementLogService : ISettlementLogService
    {
        #region Fields

        /// <summary>
        /// Calculation Log DataAccess.
        /// </summary>
        private readonly SettlementLogDataAccess _calculationLogDataAccess;

        #endregion Fields

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="SettlementLogService" /> class.
        /// </summary>
        public SettlementLogService()
        {
            this._calculationLogDataAccess = new SettlementLogDataAccess();
        }

        #endregion Constructor

        /// <summary>
        /// Load MatchInfo.
        /// </summary>
        /// <param name="param">Settlement log parameter.</param>
        /// <returns>Return SettlementLog list.</returns>
        public IList<SettlementLog> LoadMatchInfo(SettlementLogMetaModel param)
        {
            return this._calculationLogDataAccess.LoadMatchInfo(param);
        }
    }
}