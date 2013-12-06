// -----------------------------------------------------------------------
// <copyright file="IpDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DataAccess.Implement
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO.IP;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class IpDataAccess : IIpDataAccess
    {
        #region IpDataAccess Members

        /// <summary>
        /// Get All VNIP.
        /// </summary>
        /// <returns>Return IP list.</returns>
        public IList<IpVn> GetAllVnIp()
        {
            var internetProtocolVnList = new List<IpVn>();

            try
            {
                using (var db = new ObjectDb("GetIPVN"))
                {
                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = new IpVn
                                {
                                    IpFrom = reader.ToString("IpFrom"),
                                    IpTo = reader.ToString("IpTo"),
                                    IsManual = reader.ToBool("isManual")
                                };

                            internetProtocolVnList.Add(entity);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }

            return internetProtocolVnList;
        }

        /// <summary>
        /// Get IP Info.
        /// </summary>
        /// <param name="ip">IP parameter.</param>
        /// <returns>Return IP Info.</returns>
        public IpInfo GetByIp(string ip)
        {
            var internetProtocolInfo = new IpInfo();

            try
            {
                using (var db = new ObjectDb("Acc_GetIPInfo_Full"))
                {
                    db.AddInParameter("ip", DbType.String, ip);

                    using (var reader = db.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            internetProtocolInfo.KeyFrom = reader.ToInt("ipKeyFrom");
                            internetProtocolInfo.KeyTo = reader.ToInt("ipKeyTo");
                            internetProtocolInfo.From = reader.ToLong("ipFrom");
                            internetProtocolInfo.To = reader.ToLong("ipTo");
                            internetProtocolInfo.CountryCode = reader.ToString("countryShort");
                            internetProtocolInfo.CountryName = reader.ToString("countryLONG");
                            internetProtocolInfo.Region = reader.ToString("ipRegion");
                            internetProtocolInfo.City = reader.ToString("ipCity");
                            internetProtocolInfo.IsModified = !reader.IsDBNull(reader.GetOrdinal("IsModified")) &&
                                                reader.ToBool("IsModified");
                            internetProtocolInfo.IsException = !reader.IsDBNull(reader.GetOrdinal("IsException")) &&
                                                 reader.ToBool("IsException");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }

            return internetProtocolInfo;
        }

        #endregion IpDataAccess Members
    }
}