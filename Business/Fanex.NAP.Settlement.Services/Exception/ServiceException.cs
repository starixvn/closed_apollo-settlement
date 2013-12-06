// -----------------------------------------------------------------------
// <copyright file="ServiceException.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  ServiceException Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Services.Exception
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [Serializable]
    public class ServiceException : System.Exception
    {
        /// <summary>
        /// Service Name.
        /// </summary>
        private readonly string _serviceName;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ServiceException" /> class.
        /// </summary>
        /// <param name="message">Message name.</param>
        /// <param name="serviceName">Service Name.</param>
        public ServiceException(string message, string serviceName)
            : base(message)
        {
            this._serviceName = serviceName;
        }

        /// <summary>
        /// Exception Message.
        /// </summary>
        /// <value>The message of the Exception.</value>
        public override string Message
        {
            get
            {
                return string.Format("Exception when call Service: {0}. Exception Message: {1}", this._serviceName, base.Message);
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}