// -----------------------------------------------------------------------
// <copyright file="DoSingleTicketMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  DoSingle Ticket Meta Model
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.11.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.MetaModels
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class DoSingleTicketMetaModel
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="DoSingleTicketMetaModel" /> class.
        /// </summary>
        public DoSingleTicketMetaModel()
        {
            this.IsScoreDirect = false;
        }

        /// <summary>
        ///     Gets or sets MatchId.
        /// </summary>
        /// <value>The match id of the single ticket.</value>
        public int MatchId { get; set; }

        /// <summary>
        ///     Gets or sets MatchCode.
        /// </summary>
        /// <value>The match code of the single ticket.</value>
        public string MatchCode { get; set; }

        /// <summary>
        ///     Gets or sets ActionType.
        /// </summary>
        /// <value>The action type of the single ticket.</value>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sport id.
        /// </summary>
        /// <value>The sport id of the single ticket.</value>
        public int SportId { get; set; }

        /// <summary>
        ///  Gets or sets GoalRecord.
        /// </summary>
        /// <value>The goal record of the single ticket.</value>
        public string GoalRecord { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is Exist last goal and first goal.
        /// </summary>
        /// <value>The last goal and first goal of the single ticket.</value>
        public bool IsExistFgLg { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is ScoreDirect.
        /// </summary>
        /// <value>The score direct of the single ticket.</value>
        public bool IsScoreDirect { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome1Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome1Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome2Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome2Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome3Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome3Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome4Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome4Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome5Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome5Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome6Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome6Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome7Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome7Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome8Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome8Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHome9Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHome9Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHomeEI.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHomeEI { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHomeOT.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHomeOT { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHomeTotalGames.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHomeTotalGames { get; set; }

        /// <summary>
        /// Gets or sets TextBoxHomeScore.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxHtHomeScore { get; set; }

        /// <summary>
        /// Gets or sets TextBoxFinalHomeScore.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? TextBoxFinalHomeScore { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway1Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway1Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway2Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway2Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway3Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway3Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway4Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway4Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway5Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway5Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway6Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway6Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway7Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway7Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway8Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway8Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAway9Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAway9Q { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAwayEI.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAwayEI { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAwayOT.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAwayOT { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAwayTotalGames.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxAwayTotalGames { get; set; }

        /// <summary>
        /// Gets or sets TextBoxAwayScore.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxHtAwayScore { get; set; }

        /// <summary>
        /// Gets or sets TextBoxFinalAwayScore.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? TextBoxFinalAwayScore { get; set; }

        /// <summary>
        /// Gets or sets HiddenHomeScore.
        /// </summary>
        /// <value>The hidden home score of the single ticket.</value>
        public int HiddenHtHomeScore { get; set; }

        /// <summary>
        /// Gets or sets HiddenAwayScore.
        /// </summary>
        /// <value>The hidden away score of the single ticket.</value>
        public int HiddenHtAwayScore { get; set; }

        /// <summary>
        /// Gets or sets HiddenFinalHomeScore.
        /// </summary>
        /// <value>The hidden home score of the single ticket.</value>
        public int HiddenFinalHomeScore { get; set; }

        /// <summary>
        /// Gets or sets HiddenFinalAwayScore.
        /// </summary>
        /// <value>The hidden away score of the single ticket.</value>
        public int HiddenFinalAwayScore { get; set; }

        /// <summary>
        /// Gets or sets HiddenBetStatus.
        /// </summary>
        /// <value>The hidden bet status of the single ticket.</value>
        public string HiddenBetStatus { get; set; }

        /// <summary>
        /// Gets or sets LogId.
        /// </summary>
        /// <value>The log id of the single ticket.</value>
        public long LogId { get; set; }
    }
}