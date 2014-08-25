// <summary>
//  DoSingle Ticket Meta Model
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.11.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.MetaModels
{
    using Fanex.NAP.Settlement.Core.Common;

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

        public int AdminId { get; set; }

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

        public bool IsExistHtFgLg { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is ScoreDirect.
        /// </summary>
        /// <value>The score direct of the single ticket.</value>
        public bool IsScoreDirect { get; set; }

        /// <summary>
        /// Gets or sets Home1Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home1Q { get; set; }

        /// <summary>
        /// Gets or sets Home2Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home2Q { get; set; }

        /// <summary>
        /// Gets or sets Home3Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home3Q { get; set; }

        /// <summary>
        /// Gets or sets Home4Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home4Q { get; set; }

        /// <summary>
        /// Gets or sets Home5Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home5Q { get; set; }

        /// <summary>
        /// Gets or sets Home6Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home6Q { get; set; }

        /// <summary>
        /// Gets or sets Home7Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home7Q { get; set; }

        /// <summary>
        /// Gets or sets Home8Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home8Q { get; set; }

        /// <summary>
        /// Gets or sets Home9Q.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? Home9Q { get; set; }

        /// <summary>
        /// Gets or sets HomeEI.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? HomeEI { get; set; }

        /// <summary>
        /// Gets or sets HomeOT.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? HomeOT { get; set; }

        /// <summary>
        /// Gets or sets HomeTotalGames.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? HomeTotalGames { get; set; }

        /// <summary>
        /// Gets or sets HomeScore.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? HtHomeScore { get; set; }

        /// <summary>
        /// Gets or sets FinalHomeScore.
        /// </summary>
        /// <value>The home score of the single ticket.</value>
        public int? FinalHomeScore { get; set; }

        /// <summary>
        /// Gets or sets Away1Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away1Q { get; set; }

        /// <summary>
        /// Gets or sets Away2Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away2Q { get; set; }

        /// <summary>
        /// Gets or sets Away3Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away3Q { get; set; }

        /// <summary>
        /// Gets or sets Away4Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away4Q { get; set; }

        /// <summary>
        /// Gets or sets Away5Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away5Q { get; set; }

        /// <summary>
        /// Gets or sets Away6Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away6Q { get; set; }

        /// <summary>
        /// Gets or sets Away7Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away7Q { get; set; }

        /// <summary>
        /// Gets or sets Away8Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away8Q { get; set; }

        /// <summary>
        /// Gets or sets Away9Q.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? Away9Q { get; set; }

        /// <summary>
        /// Gets or sets AwayEI.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? AwayEI { get; set; }

        /// <summary>
        /// Gets or sets AwayOT.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? AwayOT { get; set; }

        /// <summary>
        /// Gets or sets AwayTotalGames.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? AwayTotalGames { get; set; }

        /// <summary>
        /// Gets or sets AwayScore.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? HtAwayScore { get; set; }

        /// <summary>
        /// Gets or sets FinalAwayScore.
        /// </summary>
        /// <value>The away score of the single ticket.</value>
        public int? FinalAwayScore { get; set; }

        /// <summary>
        /// Gets or sets LogId.
        /// </summary>
        /// <value>The log id of the single ticket.</value>
        public long LogId { get; set; }

        public string BetStatus { get; set; }

        public string IP
        {
            get
            {
                return Utility.GetClientIP;
            }
        }
    }
}