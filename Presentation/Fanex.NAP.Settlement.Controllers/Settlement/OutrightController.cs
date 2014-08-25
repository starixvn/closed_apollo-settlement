// -----------------------------------------------------------------------
// <copyright file="OutrightController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.Controllers.Settlement
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.NAP.Settlement.Core.Constants;
    using Fanex.NAP.Settlement.Core.Resources;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.Configuration;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.DTO.Permission;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.Services.Implement;
    using Fanex.NAP.Settlement.Services.Interface;
    using Fanex.NAP.Settlement.ViewModels;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class OutrightController : BaseController
    {
        #region Fields

        /// <summary>
        /// Outright Service.
        /// </summary>
        private readonly IOutrightService _outrightService;

        /// <summary>
        /// AutoSettlement Service.
        /// </summary>
        private readonly IAutoSettlementService _adminService;

        /// <summary>
        /// Sport Service.
        /// </summary>
        private readonly ISportService _sportService;

        /// <summary>
        /// League Service.
        /// </summary>
        private readonly ILeagueService _leagueService;

        private readonly ICurrentSession _currentSession;
        private IResult _result = new Result { ErrorMessage = string.Empty, ErrorCode = 0 };

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OutrightController" /> class.
        /// </summary>
        public OutrightController()
            : this(null, null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutrightController" /> class.
        /// </summary>
        /// <param name="outright">Outright Service.</param>
        /// <param name="admin">Admin Service.</param>
        /// <param name="sport">Sport Service.</param>
        /// <param name="league">League Service.</param>
        /// <param name="currentSession">Current session service.</param>
        public OutrightController(IOutrightService outright, IAutoSettlementService admin, ISportService sport, ILeagueService league, ICurrentSession currentSession)
        {
            this._outrightService = outright ?? new OutrightService();
            this._adminService = admin ?? new AutoSettlementService();
            this._sportService = sport ?? new SportService();
            this._leagueService = league ?? new LeagueService();
            this._currentSession = currentSession ?? new CurrentSession();
        }

        #endregion

        /// <summary>
        /// Get list Outright Settlement.
        /// </summary>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.OutrightSettlement, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = new OutrightMetaModel { SportsList = this._sportService.GetSportList(false) };

            // Get all league for default sport
            var leagueList = this._leagueService.GetLeagueList(new LeagueMetaModel
            {
                SportTypeId = metaModel.SportTypeId,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.FromDate,
                IsOutright = true
            });
            metaModel.LeaguesList = leagueList;

            return this.Index(metaModel);
        }

        /// <summary>
        /// Get list Outright Settlement.
        /// </summary>
        /// <param name="metaModel">Outright parameter.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.OutrightSettlement, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult Index(OutrightMetaModel metaModel)
        {
            // DisabledAutoSettle = NULL or 0  : ON
            var disabledAutoSettle = this._adminService.GetStatusAutoSettlement();
            IList<OutrightLogAction> actionLog;

            var viewModel = new OutrightViewModel
            {
                Param = metaModel,
                OutrightList = this._outrightService.GetOutrightList(metaModel, out actionLog),
                LogActionList = actionLog,
                DisabledAutoSettle = !disabledAutoSettle,
                Permission = new SettlementPermission
                {
                    CanCalculateOutright = HasPermission(_currentSession.UserId, SettlementFunctionKeys.OutrightCalculate, Constant.Policy.Read),
                    CanUncalculateOutright = HasPermission(_currentSession.UserId, SettlementFunctionKeys.OutrightUncalculate, Constant.Policy.Read),
                    CanSetAutoSettlement = HasPermission(_currentSession.UserId, SettlementFunctionKeys.AutoSettlement, Constant.Policy.Host),
                    SportsBook = new SportsBookPermission()
                    {
                        CanCalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCalculate, Constant.Policy.Host),
                    }
                }
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Get Matches By LeagueId.
        /// </summary>
        /// <param name="leagueId">League id.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.OutrightCalculate, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult Matches(int leagueId)
        {
            IList<OutrightLogAction> actionLog;
            var outright = this._outrightService.GetMatchesByLeagueId(leagueId, false, out actionLog);
            var viewModel = new OutrightLogActionViewModel
            {
                Permission = new SettlementPermission
                {
                    CanCalculateOutright = HasPermission(_currentSession.UserId, SettlementFunctionKeys.OutrightCalculate, Constant.Policy.Host)
                },
                OutrightList = outright,
                LogActionList = actionLog,
                LeagueId = leagueId
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Get Matches By LeagueId.
        /// </summary>
        /// <param name="leagueId">League id.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.OutrightUncalculate, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult SettledMatches(int leagueId)
        {
            IList<OutrightLogAction> actionLog;
            var outright = this._outrightService.GetMatchesByLeagueId(leagueId, true, out actionLog);
            var viewModel = new OutrightLogActionViewModel
            {
                Permission = new SettlementPermission
                {
                    CanUncalculateOutright = HasPermission(_currentSession.UserId, SettlementFunctionKeys.OutrightUncalculate, Constant.Policy.Host)
                },
                OutrightList = outright,
                LogActionList = actionLog,
                LeagueId = leagueId
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Action view Log.
        /// </summary>
        /// <param name="ordId">Outright id.</param>
        /// <param name="teamId">Team name id.</param>
        /// <param name="leagueId">League id.</param>
        /// <returns>Return result.</returns>
        [HttpGet]
        public ActionResult LogAction(int ordId, int teamId, int leagueId)
        {
            var viewModel = new OutrightLogActionViewModel
            {
                LogActionList = this._outrightService.GetLogAction(ordId),
                TeamId = teamId,
                LeagueId = leagueId
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Update Flag Auto Settlement.
        /// </summary>
        /// <param name="leagueid">League id.</param>
        /// <param name="isTurnOn">Turn on value.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.AutoSettlement, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult UpdateFlagOutright(int leagueid, bool isTurnOn)
        {
            var metaModel = new OutrightAutoSettleMetaModel
            {
                AdminId = _currentSession.UserId,
                LeagueId = leagueid,
                IsTurnOn = isTurnOn,
                IP = Request.ServerVariables["REMOTE_ADDR"]
            };

            var result = this._outrightService.UpdateFlagOutright(metaModel);

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UnCalculate Outright.
        /// </summary>
        /// <param name="outrightIds">The list outright is settled.</param>
        /// <param name="listSelectedTeam">The list team is selected.</param>
        /// <param name="countDeadHeat">Old count dead heat.</param>
        /// <param name="teamIds">All team Ids.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.OutrightUncalculate, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult UnCalculate(string outrightIds, string listSelectedTeam, int countDeadHeat, string teamIds)
        {
            var isError = false;
            var listTeamsSettled = outrightIds.Split('|');
            var listTeams = listSelectedTeam.Split('|');
            var i = 0;

            // Recalculate dead heat.
            var newCountDeadHeat = countDeadHeat - this.CountDeadHeat(listTeams);
            var teamName = string.Empty;
            foreach (string item in listTeamsSettled)
            {
                IResult newResult = new Result { ErrorMessage = string.Empty, ErrorCode = 0 };
                if (!string.IsNullOrEmpty(item))
                {
                    var match = item.Split(',');
                    var outrightId = Convert.ToInt32(match[0]);
                    var statusId = Convert.ToInt32(match[1]);
                    var selectedTeam = listTeams[i].Split(',')[0];

                    if ((statusId == OutrightStatus.Won && newCountDeadHeat != countDeadHeat) || selectedTeam == "1")
                    {
                        var isDeadHeat = (statusId == OutrightStatus.Won && newCountDeadHeat > 1) ? true : false;
                        var metaModel = selectedTeam == "1"
                            ? this.NewOutright(outrightId)
                            : this.NewOutright(outrightId, statusId, isDeadHeat, newCountDeadHeat);
                        newResult = this._outrightService.OutrightTrans(metaModel);
                        if (newResult.ErrorCode == 0)
                        {
                            newResult = this._outrightService.OutrightEvent(metaModel);
                        }

                        if (newResult.ErrorCode > 0)
                        {
                            teamName = this.GetTeamName(teamName, teamIds.Split('|')[i]);
                            if (!isError)
                            {
                                this._result = newResult;
                                isError = true;
                            }

                            this._result.ErrorMessage = string.Join("<br/>", newResult.ErrorMessage, teamName);
                        }
                    }
                }

                i++;
            }

            return Json(this._result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Calculate outright.
        /// </summary>
        /// <param name="outrightIds">List outright id.</param>
        /// <param name="oldOrIds">List old outright id.</param>
        /// <param name="outrightIdsFail">List outright id fail.</param>
        /// <param name="teamIds">List team id.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.OutrightCalculate, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Calculate(string outrightIds, string oldOrIds, string outrightIdsFail, string teamIds)
        {
            var isError = false;
            var list = outrightIds.Split('|'); // List outright is changed status.
            var i = 0;
            var countDeadHeat = this.CountDeadHeat(list);
            var teamName = string.Empty;
            foreach (string item in list)
            {
                IResult newResult = new Result { ErrorMessage = string.Empty, ErrorCode = 0 };
                if (!string.IsNullOrEmpty(item))
                {
                    var match = item.Split(',');
                    var statusId = Convert.ToInt32(match[1]);
                    var oldStatusId = Convert.ToInt32(oldOrIds.Split('|')[i].Split(',')[1]);
                    var outrightId = Convert.ToInt32(match[0]);
                    var isFail = outrightIdsFail.IndexOf(string.Join(",", outrightId, string.Empty)) > -1;
                    if ((statusId != OutrightStatus.Running && statusId != oldStatusId) || statusId == OutrightStatus.Won || isFail)
                    {
                        var deadHeat = (statusId == OutrightStatus.Won && countDeadHeat > 1) ? countDeadHeat : 1;
                        var isDeadHeat = (statusId == OutrightStatus.Won && countDeadHeat > 1) ? true : false;
                        var metaModel = this.NewOutright(outrightId, statusId, isDeadHeat, deadHeat);
                        newResult = this._outrightService.OutrightEvent(metaModel);
                        if (newResult.ErrorCode == 0)
                        {
                            newResult = this._outrightService.OutrightTrans(metaModel);
                            if (newResult.ErrorCode == 0 && isFail)
                            {
                                this._outrightService.ClearFailActionOutright(outrightId);
                            }
                        }

                        if (newResult.ErrorCode > 0)
                        {
                            teamName = this.GetTeamName(teamName, teamIds.Split('|')[i]);
                            if (!isError)
                            {
                                this._result = newResult;
                                isError = true;
                            }

                            this._result.ErrorMessage = string.Join("<br/>", newResult.ErrorMessage, teamName);
                        }
                    }
                }

                i++;
            }

            return Json(this._result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get status by status id.
        /// </summary>
        /// <param name="id">Status id.</param>
        /// <returns>Return value.</returns>
        private string Status(int id)
        {
            switch (id)
            {
                case OutrightStatus.Running:
                    return ViewResources.Running.ToLower();
                case OutrightStatus.Won:
                    return ViewResources.Won.ToLower();
                case OutrightStatus.Lose:
                    return ViewResources.Lose.ToLower();
                case OutrightStatus.Refund:
                    return ViewResources.Refund.ToLower();
                default:
                    return ViewResources.Running.ToLower();
            }
        }

        /// <summary>
        /// Get team name.
        /// </summary>
        /// <param name="teamName">Team name.</param>
        /// <param name="teamId">Team name id.</param>
        /// <returns>Return value.</returns>
        private string GetTeamName(string teamName, string teamId)
        {
            if (string.IsNullOrWhiteSpace(teamName))
            {
                teamName = CacheRef.GetTeamName(teamId);
            }
            else
            {
                teamName += string.Join(", ", string.Empty, CacheRef.GetTeamName(teamId));
            }

            return teamName;
        }

        /// <summary>
        /// Set new value outright.
        /// </summary>
        /// <param name="outrightId">Outright id.</param>
        /// <param name="statusId">Status id.</param>
        /// <param name="isDeadheat">Is Dead heat.</param>
        /// <param name="deadHeat">Dead Heat.</param>
        /// <returns>Return meta model.</returns>
        private OutrightCalculateMetaModel NewOutright(int outrightId, int statusId, bool isDeadheat, float deadHeat)
        {
            return new OutrightCalculateMetaModel
            {
                AdminId = _currentSession.UserId,
                MatchId = outrightId,
                Status = this.Status(statusId),
                IsDeadheat = isDeadheat,
                DeadHeat = deadHeat,
                IP = Request.ServerVariables["REMOTE_ADDR"]
            };
        }

        /// <summary>
        /// Set new value outright.
        /// </summary>
        /// <param name="outrightId">Outright id.</param>
        /// <returns>Return meta model.</returns>
        private OutrightCalculateMetaModel NewOutright(int outrightId)
        {
            return new OutrightCalculateMetaModel
            {
                AdminId = _currentSession.UserId,
                MatchId = outrightId,
                Status = CalculateAction.Uncalculate,
                IP = Request.ServerVariables["REMOTE_ADDR"]
            };
        }

        /// <summary>
        /// Count dead heat.
        /// </summary>
        /// <param name="list">Array list value.</param>
        /// <returns>Return value.</returns>
        private int CountDeadHeat(string[] list)
        {
            var countDeadHeat = 0;
            foreach (string item in list)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var matchStatus = Convert.ToInt32(item.Split(',')[1]);
                    if (matchStatus == OutrightStatus.Won)
                    {
                        countDeadHeat++;
                    }
                }
            }

            return countDeadHeat;
        }
    }
}