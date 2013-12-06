// -----------------------------------------------------------------------
// <copyright file="IpVn.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DTO.IP
{
    using System;

    /// <summary>
    /// Class IPVN.
    /// </summary>
    [Serializable]
    public class IpVn
    {
        public string IpFrom { get; set; }

        public string IpTo { get; set; }

        public bool IsManual { get; set; }
    }
}