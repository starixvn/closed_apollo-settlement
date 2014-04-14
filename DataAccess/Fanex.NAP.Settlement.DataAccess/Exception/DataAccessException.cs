// -----------------------------------------------------------------------
// <copyright file="DataAccessException.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  DataAccessException Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DataAccess.Exception
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [Serializable]
    public class DataAccessException : System.Exception
    {
        /// <summary>
        /// Stored Procedure Name.
        /// </summary>
        private readonly string _uspName;

        /// <summary>
        /// SP Parameters.
        /// </summary>
        private readonly dynamic _uspParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessException" /> class.
        /// </summary>
        /// <param name="message">Message name.</param>
        /// <param name="uspName">Stored procedure name.</param>
        /// <param name="uspParams">Stored procedure parameter.</param>
        public DataAccessException(string message, string uspName, dynamic uspParams)
            : base(message)
        {
            this._uspName = uspName;
            this._uspParams = uspParams;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="DataAccessException" /> class.
        /// </summary>
        /// <param name="message">Message name.</param>
        /// <param name="uspName">Stored procedure name.</param>
        public DataAccessException(string message, string uspName)
            : base(message)
        {
            this._uspName = uspName;
            this._uspParams = null;
        }

        /// <summary>
        /// Exception Message.
        /// </summary>
        /// <value>The message of the Exception.</value>
        public override string Message
        {
            get
            {
                return this._uspParams == null
                           ? string.Format("Exception when call usp {0}. Exception: {1}", this._uspName, base.Message)
                           : string.Format("Exception when call usp {0}. Params: {1}. Exception: {2}", this._uspName, this._uspParams, base.Message);
            }
        }
    }
}