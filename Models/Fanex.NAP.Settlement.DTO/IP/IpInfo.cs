// -----------------------------------------------------------------------
// <copyright file="IpInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>

namespace Fanex.NAP.Settlement.DTO.IP
{
    using System;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [Serializable]
    public class IpInfo
    {
        public string Ip { get; set; }

        public int KeyFrom { get; set; }

        public int KeyTo { get; set; }

        public long To { get; set; }

        public long From { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public bool IsModified { get; set; }

        public bool IsException { get; set; }
    }
}