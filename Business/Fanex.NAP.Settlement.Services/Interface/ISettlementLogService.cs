// -----------------------------------------------------------------------
// <copyright file="ISettlementLogService.cs" company="Nexcel Solutions Vietnam">
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
namespace Fanex.NAP.Settlement.Services.Interface
{
    #region

    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.MetaModels;

    #endregion

    /// <summary>
    ///     Match's log service.
    /// </summary>
    public interface ISettlementLogService
    {
        /// <summary>
        /// Load MatchInfo.
        /// </summary>
        /// <param name="param">Settlement log parameter.</param>
        /// <returns>Return SettlementLog list.</returns>
        IList<SettlementLog> LoadMatchInfo(SettlementLogMetaModel param);
    }
}