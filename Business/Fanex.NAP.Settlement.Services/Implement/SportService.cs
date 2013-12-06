// -----------------------------------------------------------------------
// <copyright file="SportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Services.Implement
{
    #region

    using System.Collections.Generic;
    using Fanex.Caching;
    using Fanex.NAP.Settlement.DataAccess.Implement;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.Services.Interface;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SportService : ISportService
    {
        #region Fields

        /// <summary>
        /// Sport List CacheKey.
        /// </summary>
        private const string SportListCacheKey = "SportListCacheKey";

        /// <summary>
        /// Sport DataAccess.
        /// </summary>
        private readonly SportDataAccess _sportDataAccess;

        /// <summary>
        /// Cache Manager.
        /// </summary>
        private readonly ICache _cacheManager;

        #endregion Fields

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="SportService" /> class.
        /// </summary>
        public SportService()
        {
            _sportDataAccess = new SportDataAccess();
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        #endregion Constructor

        /// <summary>
        /// Get all sports of SportsBooks.
        /// </summary>
        /// <param name="allSport">False to get sports for SportsBook only.</param>
        /// <returns>Return sport list.</returns>
        public IList<Sport> GetSportList(bool allSport = false)
        {
            var sports = _cacheManager.Get<IList<Sport>>(SportListCacheKey);
            if (sports == null || sports.Count == 0)
            {
                sports = _sportDataAccess.GetSportList(allSport);

                if (sports != null && sports.Count > 0)
                {
                    _cacheManager.Set<IList<Sport>>(SportListCacheKey, sports);
                }
            }

            return sports;
        }
    }
}