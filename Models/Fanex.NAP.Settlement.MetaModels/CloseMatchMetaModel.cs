// --------------------------------------------------------------------------
// <copyright file="CloseMatchMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.12.10">Created</change>
// </history>
namespace Fanex.NAP.Settlement.MetaModels
{
    /// <summary>
    ///  Parameter close match.
    /// </summary>
    public class CloseMatchMetaModel
    {
        public int Creator { get; set; }

        public int MatchId { get; set; }

        public string IP { get; set; }
    }
}