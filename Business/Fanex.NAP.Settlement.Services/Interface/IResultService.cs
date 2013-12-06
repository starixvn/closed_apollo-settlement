// -----------------------------------------------------------------------
// <copyright file="IResultService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Result Service Interface
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.10.03">Created</change>
//  <change who="Kaden" date="2012.10.03">Implement Interface for reult page</change>
// </history>

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
    public interface IResultService
    {
        IList<Match> SearchMatch(ResultMetaModel request);

        IList<Match> SearchMatchNumberGame(ResultMetaModel request);

        int TotalTopRace(IList<Match> listMatch, string groupMatch, int leagueId);

        string HorseRacingScrStatus(IList<Match> listMatch, string groupMatch, int leagueId);

        string HorseRacingTfStatus(IList<Match> listMatch, string groupMatch, int leagueId);
    }
}