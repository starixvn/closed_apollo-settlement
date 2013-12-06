// -----------------------------------------------------------------------
// <copyright file="IResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Class IResult
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.DTO.DBResult
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class Result.
    /// </summary>
    public interface IResult
    {
        int ErrorCode { get; set; }

        string ErrorMessage { get; set; }

        bool IsSuccessful { get; }
    }
}