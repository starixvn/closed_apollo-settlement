// -----------------------------------------------------------------------
// <copyright file="UrlExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Formater Functions
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.23">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Core.HtmlExtension
{
    using System.Configuration;
    using System.Web.Mvc;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class UrlExtension
    {
        /// <summary>
        /// Content Link.
        /// </summary>
        /// <param name="helper">Url helper.</param>
        /// <param name="contentPath">Path Content.</param>
        /// <returns>Return link.</returns>
        public static string ContentLink(this UrlHelper helper, string contentPath)
        {
            var requestUrl = helper.RequestContext.HttpContext.Request.Url;
            return requestUrl == null
                ? string.Empty
                : string.Format("{0}://{1}{2}?{3}", requestUrl.Scheme, requestUrl.Authority, contentPath, ConfigurationManager.AppSettings["JsVersion"]);
        }

        /// <summary>
        /// Absolute Url.
        /// </summary>
        /// <param name="helper">Url helper.</param>
        /// <param name="url">Url value.</param>
        /// <returns>Return link.</returns>
        public static string AbsoluteUrl(this UrlHelper helper, string url)
        {
            var requestUrl = helper.RequestContext.HttpContext.Request.Url;

            return requestUrl == null
                ? string.Empty
                : string.Format("{0}://{1}{2}", requestUrl.Scheme, requestUrl.Authority, url);
        }
    }
}