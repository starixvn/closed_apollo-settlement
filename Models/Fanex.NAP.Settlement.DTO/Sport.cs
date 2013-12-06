// -----------------------------------------------------------------------
// <copyright file="Sport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  DTO Sport
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.DTO
{
    using System;

    /// <summary>
    /// Class Sport type.
    /// </summary>
    [Serializable]
    public class Sport
    {
        public byte SportType { get; set; }

        public string SportName { get; set; }
    }
}