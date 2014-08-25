// -----------------------------------------------------------------------
// <copyright file="IAutoSettlementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.24">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Services.Interface
{
    using Fanex.NAP.Settlement.DTO.DBResult;

    /// <summary>
    /// IAuto Settlement Service.
    /// </summary>
    public interface IAutoSettlementService
    {
        IResult UpdateStatusAutoSettlement(int adminId, int matchId, bool turnOn, string ip);

        bool GetStatusAutoSettlement();

        IResult ClearFailActionAutoSettlement(int adminid, int matchId, string matchIds);
    }
}