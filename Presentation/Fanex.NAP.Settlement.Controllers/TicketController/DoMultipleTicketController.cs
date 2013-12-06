// -----------------------------------------------------------------------
// <copyright file="DoMultipleTicketController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Do Multiple Ticket Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.11.13">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.Controllers.TicketController
{
    #region

    using System;
    using System.Collections;
    using System.Web.Mvc;
    using Fanex.Logging;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Settlement.DTO.Configuration;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.DTO.Domain;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.Services.Implement;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [NapSuperAuth]
    public class DoMultipleTicketController : TicketBaseController
    {
        #region Calculate Mix Parlay

        /// <summary>
        /// Calculate Mix Parlay.
        /// </summary>
        /// <param name="model">Model Multiple ticket.</param>
        /// <returns>View result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookCalculateMixParlay, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult CalculateMixParlay(DoMultipleTicketMetaModel model)
        {
            IResult result = new Result();
            var inputParameters = new Hashtable();

            inputParameters.Add("matchids", model.MatchIds);
            inputParameters.Add("creator", CurrentSession.UserId);
            inputParameters.Add("action", CalculateAction.CalculateAll);
            inputParameters.Add("ip", HttpContext.Request.UserHostAddress);
            result = CalculateService.Calculate(inputParameters, CalculationType.CalculateParlay);

            if (result.IsSuccessful)
            {
                var adminService = new AutoSettlementService();
                result = adminService.ClearFailActionAutoSettlement(this.CurrentSession.UserId, 0, model.MatchIds);
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Calculate Mix Parlay

        #region Refund Parlay

        /// <summary>
        ///     Refund Parlay.
        /// </summary>
        /// <param name="model">Meta Model.</param>
        /// <returns>View result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookRefund, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult RefundParlay(DoMultipleTicketMetaModel model)
        {
            IResult result = new Result();

            result = this.RefundParlay(model.ActionType.ToLowerInvariant(), model.MatchId);

            if (result.IsSuccessful)
            {
                var adminService = new AutoSettlementService();
                result = adminService.ClearFailActionAutoSettlement(this.CurrentSession.UserId, 0, model.MatchIds);
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Refund Parlay.
        /// </summary>
        /// <param name="type">Action type.</param>
        /// <param name="matchId">Match Id parameter.</param>
        /// <returns>Return result.</returns>
        [NonAction]
        private IResult RefundParlay(string type, int matchId = 0)
        {
            var inputParameters = new Hashtable();

            inputParameters.Add("matchids", matchId);
            inputParameters.Add("creator", CurrentSession.UserId);
            inputParameters.Add("ip", HttpContext.Request.UserHostAddress);

            switch (type)
            {
                case CalculationType.RefundParlayHalfTime:
                    inputParameters.Add("action", CalculateAction.RefundHalfTime);
                    break;
                case CalculationType.RefundParlayFullTime:
                    inputParameters.Add("action", CalculateAction.RefundFullTime);
                    break;
                case CalculationType.RefundParlayAll:
                    inputParameters.Add("action", CalculateAction.Refund);
                    break;
            }

            return CalculateService.Calculate(inputParameters, type);
        }

        #endregion
    }
}