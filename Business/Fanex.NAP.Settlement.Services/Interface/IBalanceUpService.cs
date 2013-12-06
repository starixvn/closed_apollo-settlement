// -----------------------------------------------------------------------
// <copyright file="IBalanceUpService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.24">Created</change>
// </history>
namespace Fanex.NAP.Settlement.Services.Interface
{
    #region

    using Fanex.NAP.Settlement.DTO.Configuration;

    #endregion

    /// <summary>
    /// IBalance Up Service.
    /// </summary>
    public interface IBalanceUpService
    {
        #region Public Properties

        /// <summary>
        ///     Gets GetBalanceUp.
        /// </summary>
        /// <value>The balance up of the BalanceUpService.</value>
        BalanceUp GetBalanceUp { get; }

        #endregion
    }
}