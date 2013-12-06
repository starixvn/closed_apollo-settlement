// -----------------------------------------------------------------------
// <copyright file="CacheKey.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  BetStatus Access Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DTO.Configuration
{
    /// <summary>
    /// Class CacheKey.
    /// </summary>
    public static class CacheKey
    {
        #region common

        /// <summary>
        /// Memory Cache Common Balance Up.
        /// </summary>
        public const string MemorycacheCommonBalanceup = "MemoryCache_Common_BalanceUp";

        /// <summary>
        /// Memory Cache Command.
        /// </summary>
        public const string MemoryCacheCommand = "MemoryCache_Command";

        /// <summary>
        /// Memory Cache Database List.
        /// </summary>
        public const string MemoryCacheDatabaseList = "MemoryCache_DatabaseList";

        #endregion common
    }
}