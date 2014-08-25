// -----------------------------------------------------------------------
// <copyright file="IAutoSettlementDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.24">Created</change>
// </history>
namespace Fanex.NAP.Settlement.DataAccess.Interface
{
    using Fanex.NAP.Settlement.DTO.DBResult;

    /// <summary>
    /// Interface Auto Settlement DataAccess.
    /// </summary>
    public interface IAutoSettlementDataAccess
    {
        IResult UpdateStatusAutoSettlement(int adminId, int matchId, bool turnOn, string ip);

        bool GetStatusAutoSettlement();

        IResult ClearFailActionAutoSettlement(int adminId, int matchId, string matchIds);
    }
}