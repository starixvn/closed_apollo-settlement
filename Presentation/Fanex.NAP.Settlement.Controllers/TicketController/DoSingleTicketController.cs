// -----------------------------------------------------------------------
// <copyright file="DoSingleTicketController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Do Single Ticket Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.11.14">Created and Implement</change>
//  <change who="Kaden" date="2012.11.29">Refactor code</change>
// </history>

namespace Fanex.NAP.Settlement.Controllers.TicketController
{
    #region

    using System;
    using System.Web.Mvc;
    using Fanex.Logging;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Settlement.DTO;
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
    public partial class DoSingleTicketController : TicketBaseController
    {
        #region Fields

        #endregion

        /// <summary>
        /// We need split two functions Calculate and Un-Calculate so that we can check permission on action.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <returns>View result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookCalculate, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Calculate(DoSingleTicketMetaModel model)
        {
            var result = this.CalculateUpdate(model);

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Refund
        /// This function also include RefundHalfTime, RefundFullTime and RefundAll.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <returns>View result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookRefund, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Refund(DoSingleTicketMetaModel model)
        {
            var result = new Result();
            try
            {
                result = this.RefundCommand(model) as Result;
                if (result.IsSuccessful)
                {
                    var adminService = new AutoSettlementService();
                    result = adminService.ClearFailActionAutoSettlement(this.CurrentSession.UserId, model.MatchId, string.Empty) as Result;
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Un Calculate.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <returns>View result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookUncalculate, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult UnCalculate(DoSingleTicketMetaModel model)
        {
            IResult result = new Result();
            result = this.UnCalculateCommand(model);

            if (result.IsSuccessful)
            {
                var adminService = new AutoSettlementService();
                result = adminService.ClearFailActionAutoSettlement(this.CurrentSession.UserId, model.MatchId, string.Empty);
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Calculate update.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <returns>Return result.</returns>
        private IResult CalculateUpdate(DoSingleTicketMetaModel model)
        {
            var result = new Result();
            try
            {
                var type = model.ActionType.ToLowerInvariant();
                switch (type)
                {
                    case CalculationType.CalculateHalfTime:
                    case CalculationType.CalculateFullTime:
                    case CalculationType.CalculateAll:
                        result = this.CalculateCommand(model) as Result;
                        break;

                    case CalculationType.CalculateFgng:
                        result = this.CalculateFglg(model) as Result;
                        break;
                }

                if (result.IsSuccessful)
                {
                    var adminService = new AutoSettlementService();
                    result = adminService.ClearFailActionAutoSettlement(this.CurrentSession.UserId, model.MatchId, string.Empty) as Result;
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }

            return result;
        }
    }
}