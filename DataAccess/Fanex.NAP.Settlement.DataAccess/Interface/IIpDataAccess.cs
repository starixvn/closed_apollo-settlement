// -----------------------------------------------------------------------
// <copyright file="IIpDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DataAccess.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO.IP;

    /// <summary>
    /// Interface IP DataAccess.
    /// </summary>
    public interface IIpDataAccess
    {
        /// <summary>
        /// Get All VNIP.
        /// </summary>
        /// <returns>Return IP list.</returns>
        IList<IpVn> GetAllVnIp();

        /// <summary>
        /// Get IP Info.
        /// </summary>
        /// <param name="ip">IP parameter.</param>
        /// <returns>Return IP Info.</returns>
        IpInfo GetByIp(string ip);
    }
}