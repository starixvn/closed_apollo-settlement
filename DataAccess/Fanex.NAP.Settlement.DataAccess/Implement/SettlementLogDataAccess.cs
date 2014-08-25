// -----------------------------------------------------------------------
// <copyright file="SettlementLogDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DataAccess.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SettlementLogDataAccess : ISettlementLogDataAccess
    {
        /// <summary>
        /// Load MatchInfo.
        /// </summary>
        /// <param name="param">Settlement log parameter.</param>
        /// <returns>Return SettlementLog list.</returns>
        public IList<SettlementLog> LoadMatchInfo(SettlementLogMetaModel param)
        {
            var uspName = "Acc_UserLog_Settle_MatchSelByCode";

            var calculationLogList = new List<SettlementLog>();
            var list = new List<SettlementLog>();
            var uspParams = new
                {
                    param.MatchCode,
                    param.FromDate,
                    param.ToDate,
                    param.BalanceUpDay
                };

            using (var db = new ObjectDb(uspName))
            {
                list = db.Query<SettlementLog>(uspParams).ToList();
            }

            foreach (var item in list)
            {
                var calculationLogDetailList = this.LoadMatchInfoDetails(item.MatchId);

                // Store match log (match log include atleast 1 match log detail)
                if (calculationLogDetailList.Count > 0)
                {
                    var calculationLog = new SettlementLog
                    {
                        HomeId = item.HomeId,
                        AwayId = item.AwayId,
                        LeagueId = item.LeagueId,
                        SportType = item.SportType,
                        MatchId = item.MatchId,
                        EventDate = item.EventDate,
                        MatchLogDetailsList = calculationLogDetailList,
                    };
                    calculationLogList.Add(calculationLog);
                }
            }

            return calculationLogList;
        }

        /// <summary>
        /// Match log detail.
        /// </summary>
        /// <param name="matchId">Match id parameter.</param>
        /// <returns>Return log list.</returns>
        public IList<SettlementLogDetails> LoadMatchInfoDetails(int matchId)
        {
            var uspName = "Acc_UserLog_Settle_MatchSelByID";

            using (var db = new ObjectDb(uspName))
            {
                return db.Query<SettlementLogDetails>(new { matchId }).ToList();
            }
        }
    }
}