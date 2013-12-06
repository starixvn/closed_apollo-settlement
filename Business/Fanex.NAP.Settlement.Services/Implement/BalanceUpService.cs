// -----------------------------------------------------------------------
// <copyright file="BalanceUpService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Services.Implement
{
    #region

    using System;
    using Fanex.Caching;
    using Fanex.Logging;
    using Fanex.NAP.Settlement.DataAccess.Implement;
    using Fanex.NAP.Settlement.DTO.Configuration;
    using Fanex.NAP.Settlement.Services.Exception;
    using Fanex.NAP.Settlement.Services.Interface;

    #endregion

    /// <summary>
    /// Balance up Service.
    /// </summary>
    public class BalanceUpService : IBalanceUpService
    {
        #region Constants and Fields

        /// <summary>
        ///     The cache timeout minute.
        /// </summary>
        private const int CacheTimeoutMinute = 1;

        /// <summary>
        ///     The _balance up data access.
        /// </summary>
        private readonly BalanceUpDataAccess _balanceUpDataAccess;

        /// <summary>
        /// Cache Manager.
        /// </summary>
        private readonly ICache _cacheManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BalanceUpService" /> class.
        /// </summary>
        public BalanceUpService()
        {
            this._balanceUpDataAccess = new BalanceUpDataAccess();
            this._cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        #endregion

        /// <summary>
        /// Gets BalanceUp day.
        /// </summary>
        /// <value>The message of the Exception.</value>
        public BalanceUp GetBalanceUp
        {
            get
            {
                var balanceUp = _cacheManager.Get<BalanceUp>(CacheKey.MemorycacheCommonBalanceup);

                if (balanceUp != null)
                {
                    return balanceUp;
                }

                balanceUp = this._balanceUpDataAccess.GetBalanceUpDay();
                _cacheManager.Set(CacheKey.MemorycacheCommonBalanceup, balanceUp, DateTime.Now.AddMinutes(CacheTimeoutMinute));

                return balanceUp;
            }
        }
    }
}