// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class Result : IResult
    {
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets Error Message associates with error code.
        /// </summary>
        /// <value>The message of error result.</value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets a value indicating whether.
        /// </summary>
        /// <value>The status of error result.</value>
        public bool IsSuccessful
        {
            get { return string.IsNullOrEmpty(this.ErrorMessage) || (this.ErrorCode == 0); }
        }
    }
}