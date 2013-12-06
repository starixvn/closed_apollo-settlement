// -----------------------------------------------------------------------
// <copyright file="LogController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.29">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.Controllers.Settlement
{
    using System.Web.Mvc;
    using Core.Web.Attribute;
    using DTO.Configuration;
    using Fanex.NAP.Settlement.Services.Implement;
    using Fanex.NAP.Settlement.Services.Interface;
    using Fanex.NAP.Settlement.Web.Configuration;
    using MetaModels;
    using ViewModels;

    /// <summary>
    /// Log Controller.
    /// </summary>
    [NapSuperAuth]
    [NapAuth(FunctionKey = SettlementFunctionKeys.ViewLogSettlement)]
    public class LogController : BaseController
    {
        #region Fields

        /// <summary>
        /// Calculate Service.
        /// </summary>
        private readonly ICalculateService _calculateService;

        /// <summary>
        /// IP Service.
        /// </summary>
        private readonly IIpService _internetProtocolService;

        /// <summary>
        /// BalanceUp Service.
        /// </summary>
        private readonly IBalanceUpService _balanceUpService;

        /// <summary>
        /// SettlementLog Service.
        /// </summary>
        private readonly ISettlementLogService _settlementLogService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LogController" /> class.
        /// </summary>
        public LogController()
            : this(null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogController" /> class.
        /// </summary>
        /// <param name="calcService">Calculate Service.</param>
        /// <param name="ipService">IP Service.</param>
        /// <param name="balanceService">Balance Up Service.</param>
        /// <param name="logService">Settlement Log Service.</param>
        public LogController(ICalculateService calcService, IpService ipService, IBalanceUpService balanceService, ISettlementLogService logService)
        {
            this._calculateService = calcService ?? new CalculateService();
            this._internetProtocolService = ipService ?? new IpService();
            this._balanceUpService = balanceService ?? new BalanceUpService();
            this._settlementLogService = logService ?? new SettlementLogService();
        }

        #endregion Constructor

        /// <summary>
        /// Action View log.
        /// </summary>
        /// <param name="param">Log parameter.</param>
        /// <returns>Return view result.</returns>
        public ActionResult Index(SettlementLogMetaModel param)
        {
            return this.View(this.LoadData(param));
        }

        /// <summary>
        /// Action View log.
        /// </summary>
        /// <param name="param">Log parameter.</param>
        /// <returns>Return view result.</returns>
        public ActionResult Popup(SettlementLogMetaModel param)
        {
            return this.View(this.LoadData(param));
        }

        /// <summary>
        /// Action View IP.
        /// </summary>
        /// <param name="ip">IP parameter.</param>
        /// <returns>Return view result.</returns>
        [HttpGet]
        public ActionResult ViewIpInfo(string ip)
        {
            var internetProtocolInfo = this._internetProtocolService.GetIpInfo(ip);
            return this.View(internetProtocolInfo);
        }

        /// <summary>
        /// View list action on Popup.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <param name="sportId">SportType Id.</param>
        /// <returns>Return action result.</returns>
        [HttpGet]
        public ActionResult ReferenceActions(int matchId, int sportId)
        {
            var listAction = this._calculateService.GetListConfirmedAction(matchId);
            var match = this._calculateService.GetMatchDetail(matchId, true);

            // Default is Soccer
            var sportViewSetting = SportViewSettingFactory.Create(sportId);

            var model = new LogActionViewModel
            {
                MatchLogList = listAction,
                CurrentMatch = match,
                GroupSport = sportViewSetting.GroupSport,
                Template = sportViewSetting.Template
            };
            return this.View(model);
        }

        /// <summary>
        /// Load Match Info.
        /// </summary>
        /// <param name="param">Log parameter.</param>
        /// <returns>Return view model.</returns>
        private SettlementLogViewModel LoadData(SettlementLogMetaModel param)
        {
            // Get Balance up day
            param.BalanceUpDay = this._balanceUpService.GetBalanceUp.BalanceUpDay;

            var mainData = this._settlementLogService.LoadMatchInfo(param);

            var model = new SettlementLogViewModel
            {
                Param = param,
                MainData = mainData
            };

            return model;
        }
    }
}