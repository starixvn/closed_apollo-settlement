// --------------------------------------------------------------------------
// <copyright file="AutoSettlementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.29">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Controllers.Settlement
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Settlement.DTO.Configuration;
    using Fanex.NAP.Settlement.Services.Implement;
    using Fanex.NAP.Settlement.Services.Interface;

    /// <summary>
    /// Controller Auto Settlement.
    /// </summary>
    public class AutoSettlementController : BaseController
    {
        #region Fields

        /// <summary>
        /// Settlement Service.
        /// </summary>
        private readonly IAutoSettlementService _adminService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSettlementController" /> class.
        /// </summary>
        public AutoSettlementController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSettlementController" /> class.
        /// </summary>
        /// <param name="adminService">Admin service.</param>
        public AutoSettlementController(IAutoSettlementService adminService)
        {
            this._adminService = adminService ?? new AutoSettlementService();
        }

        #endregion Constructor

        /// <summary>
        /// Update status : Auto Settle all matches or not.
        /// </summary>
        /// <param name="isTurnOn">0: none, 1: auto settle.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <returns>Result updated.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.AutoSettlement, Policy = Constant.Policy.Host)]
        public ActionResult UpdateStatusAll(int isTurnOn, int matchId)
        {
            var userId = this.CurrentSession.UserId;
            string ip = HttpContext.Request.UserHostAddress;
            var result = this._adminService.UpdateStatusAutoSettlement(userId, matchId, isTurnOn == 1, ip);
            return this.Json(result);
        }
    }
}