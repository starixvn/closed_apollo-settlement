// -----------------------------------------------------------------------
// <copyright file="IIpService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Services.Interface
{
    #region

    using Fanex.NAP.Settlement.DTO.IP;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IIpService
    {
        /// <summary>
        /// Get IP Info.
        /// </summary>
        /// <param name="ip">IP parameter.</param>
        /// <returns>Return IP Info.</returns>
        IpInfo GetIpInfo(string ip);
    }
}