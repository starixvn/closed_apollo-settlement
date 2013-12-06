// -----------------------------------------------------------------------
// <copyright file="IBalanceUpDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.24">Created</change>
// </history>
namespace Fanex.NAP.Settlement.DataAccess.Interface
{
    #region

    using Fanex.NAP.Settlement.DTO.Configuration;

    #endregion

    /// <summary>
    /// Interface Balance Up DataAccess.
    /// </summary>
    public interface IBalanceUpDataAccess
    {
        /// <summary>
        /// Gets BalanceUp day.
        /// </summary>
        /// <returns>Return balance up info.</returns>
        BalanceUp GetBalanceUpDay();
    }
}