// -----------------------------------------------------------------------
// <copyright file="AutoSettlementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.24">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Services.Implement
{
    using Fanex.NAP.Settlement.DataAccess.Implement;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.Services.Interface;

    /// <summary>
    /// Auto Settlement Service.
    /// </summary>
    public class AutoSettlementService : IAutoSettlementService
    {
        /// <summary>
        /// Admin DataAccess.
        /// </summary>
        private IAutoSettlementDataAccess _adminDataAccess;

        /// <summary>
        ///  Initializes a new instance of the <see cref="AutoSettlementService" /> class.
        /// </summary>
        public AutoSettlementService()
        {
            _adminDataAccess = new AutoSettlementDataAccess();
        }

        /// <summary>
        /// Update status AutoSettlement.
        /// </summary>
        /// <param name="adminId">AdminId by.</param>
        /// <param name="matchId">Match Id parameter.</param>
        /// <param name="turnOn">Turn on parameter.</param>
        /// <param name="ip">Internet protocol.</param>
        /// <returns>Return result.</returns>
        public IResult UpdateStatusAutoSettlement(int adminId, int matchId, bool turnOn, string ip)
        {
            IResult result = new Result();
            if (matchId > 0)
            {
                var globalStatusAutoSettlement = this.GetStatusAutoSettlement();
                if (!globalStatusAutoSettlement)
                {
                    result = new Result()
                    {
                        ErrorCode = 150,
                        ErrorMessage = "You are unable to turn ON/OFF the auto settlement"
                                    + " for this match as the global switch for auto settlement has been turned OFF by other Admins."
                    };

                    return result;
                }
            }

            result = _adminDataAccess.UpdateStatusAutoSettlement(adminId, matchId, turnOn, ip);
            return result;
        }

        /// <summary>
        /// Get status AutoSettlement.
        /// </summary>
        /// <returns>Return boolean.</returns>
        public bool GetStatusAutoSettlement()
        {
            return _adminDataAccess.GetStatusAutoSettlement();
        }

        /// <summary>
        /// Clear fail action AutoSettlement.
        /// </summary>
        /// <param name="adminId">AdminId by.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <param name="matchIds">List match ids.</param>
        /// <returns>Return result.</returns>
        public IResult ClearFailActionAutoSettlement(int adminId, int matchId, string matchIds)
        {
            return _adminDataAccess.ClearFailActionAutoSettlement(adminId, matchId, matchIds);
        }
    }
}