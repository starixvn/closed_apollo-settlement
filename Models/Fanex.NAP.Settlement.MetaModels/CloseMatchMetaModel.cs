// <history>
//   <change who="Kaden.Trinh" date="2013.12.10">Created</change>
// </history>

namespace Fanex.NAP.Settlement.MetaModels
{
    using Fanex.NAP.Settlement.Core.Common;

    /// <summary>
    ///  Parameter close match.
    /// </summary>
    public class CloseMatchMetaModel
    {
        public int AdminId { get; set; }

        public int MatchId { get; set; }

        public string IP
        {
            get
            {
                return Utility.GetClientIP;
            }
        }
    }
}