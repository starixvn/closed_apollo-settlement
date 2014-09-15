// -----------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Core.Constants
{
    #region

    using RemotingRef;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class CacheRef
    {
        /// <summary>
        /// Language Code.
        /// </summary>
        private const string LanguageCode = "en";

        /// <summary>
        /// Get BetType.
        /// </summary>
        /// <param name="bettypeId">Bet type id.</param>
        /// <returns>Return name.</returns>
        public static string GetBettype(object bettypeId)
        {
            return RefData.GetBetTypeName(bettypeId.ToString(), LanguageCode, string.Empty);
        }

        /// <summary>
        /// Get SportName.
        /// </summary>
        /// <param name="sportId">Sport type id.</param>
        /// <returns>Return name.</returns>
        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), LanguageCode, string.Empty);
        }

        /// <summary>
        /// Get TeamName.
        /// </summary>
        /// <param name="teamId">Team name id.</param>
        /// <returns>Return name.</returns>
        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), LanguageCode, string.Empty);
        }

        /// <summary>
        /// Get LeagueName.
        /// </summary>
        /// <param name="leagueId">League id.</param>
        /// <returns>Return name.</returns>
        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);
        }

        /// <summary>
        /// Get Function IndexName.
        /// </summary>
        /// <param name="indexId">Index id parameter.</param>
        /// <returns>Return name.</returns>
        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(indexId.ToString(), LanguageCode);
        }

        /// <summary>
        /// Get Horse TeamName.
        /// </summary>
        /// <param name="codeId">Code id parameter.</param>
        /// <returns>Return name.</returns>
        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), LanguageCode);
        }

        /// <summary>
        /// Get Odds TypeName.
        /// </summary>
        /// <param name="oddsTypeId">Odds type id.</param>
        /// <returns>Return name.</returns>
        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), LanguageCode);
        }

        /// <summary>
        /// Get Casino BetItem.
        /// </summary>
        /// <param name="betStatusId">Bet status id.</param>
        /// <returns>Return name.</returns>
        public static string GetCasinoBetItem(object betStatusId)
        {
            return RefData.GetCasinoBetItem(betStatusId.ToString(), LanguageCode, string.Empty);
        }

        /// <summary>
        /// Get Roulette BetItem.
        /// </summary>
        /// <param name="betStatusId">Bet status id.</param>
        /// <returns>Return name.</returns>
        public static string GetRouletteBetItem(object betStatusId)
        {
            return RefData.GetRouletteBetItem(betStatusId.ToString(), LanguageCode, string.Empty);
        }
    }
}