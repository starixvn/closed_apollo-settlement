// -----------------------------------------------------------------------
// <copyright file="IpService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.Services.Implement
{
    #region

    using Fanex.Common.Helper;
    using Fanex.Logging;
    using Fanex.NAP.Settlement.DataAccess.Implement;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO.IP;
    using Fanex.NAP.Settlement.Services.Interface;

    #endregion

    /// <summary>
    /// IP Service.
    /// </summary>
    public class IpService : IIpService
    {
        /// <summary>
        ///  Data Access.
        /// </summary>
        private readonly IIpDataAccess _dataAccess;

        /// <summary>
        ///  Initializes a new instance of the <see cref="IpService" /> class.
        /// </summary>
        public IpService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IpService" /> class.
        /// </summary>
        /// <param name="dataAccess">IP data access.</param>
        public IpService(IIpDataAccess dataAccess)
        {
            this._dataAccess = dataAccess ?? new IpDataAccess();
        }

        /// <summary>
        /// Get IP Info.
        /// </summary>
        /// <param name="ip">IP parameter.</param>
        /// <returns>Return IP Info.</returns>
        public IpInfo GetIpInfo(string ip)
        {
            // check parameters
            Check.Argument.IsValidIp(ip, "ip");

            var internetProtocolInfo = new IpInfo();

            try
            {
                internetProtocolInfo = this._dataAccess.GetByIp(ip);
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }

            internetProtocolInfo.Ip = ip;

            return internetProtocolInfo;
        }
    }
}