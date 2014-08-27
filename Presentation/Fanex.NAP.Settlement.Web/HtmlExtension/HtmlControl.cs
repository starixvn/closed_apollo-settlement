// -----------------------------------------------------------------------
// <copyright file="HtmlControl.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Html InputExtensions Control
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.10.29">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Core.HtmlExtension
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.NAP.Settlement.Core.Common;
    using Fanex.NAP.Settlement.Core.Constants;
    using Fanex.NAP.Settlement.Core.Resources;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class HtmlControl
    {
        /// <summary>
        /// Extension TextBox.
        /// </summary>
        /// <param name="htmlHelper">Html helper.</param>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        /// <param name="disabled">Disabled value.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Return Html helper.</returns>
        public static MvcHtmlString TextBox(this HtmlHelper htmlHelper, string name, object value, bool disabled, object htmlAttributes)
        {
            return InputHelper(htmlHelper, name, value, disabled, htmlAttributes);
        }

        /// <summary>
        /// Extension Label.
        /// </summary>
        /// <param name="htmlHelper">Html helper.</param>
        /// <param name="value">Attribute value.</param>
        /// <returns>Return Html helper.</returns>
        public static MvcHtmlString Label(this HtmlHelper htmlHelper, object value)
        {
            return MvcHtmlString.Create(value.ToString());
        }

        /// <summary>
        /// Extension TextBox or Label.
        /// </summary>
        /// <param name="html">Html helper.</param>
        /// <param name="page">Page type.</param>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        /// <param name="disabled">Disabled value.</param>
        /// <param name="att">Html attributes.</param>
        /// <returns>Return Html helper.</returns>
        public static MvcHtmlString TextBoxOrLabel(this HtmlHelper html, PageType page, string name, object value, bool disabled, object att)
        {
            switch (page)
            {
                case PageType.Calc:
                    return TextBox(html, name, value, disabled, att);
                default:
                    return Label(html, Utility.GetScore(value));
            }
        }

        /// <summary>
        /// Extension get action name.
        /// </summary>
        /// <param name="htmlHelper">Html Helper.</param>
        /// <param name="actionId">Action Id.</param>
        /// <returns>Action name.</returns>
        public static MvcHtmlString ActionName(this HtmlHelper htmlHelper, byte actionId)
        {
            var action = Utility.GetAction(actionId);

            return MvcHtmlString.Create(action);
        }

        /// <summary>
        /// Helper methods.
        /// </summary>
        /// <param name="htmlHelper">Html helper.</param>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        /// <param name="disabled">Disabled value.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Return Html helper.</returns>
        private static MvcHtmlString InputHelper(this HtmlHelper htmlHelper, string name, object value, bool disabled, object htmlAttributes)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            builder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Text));
            builder.MergeAttribute("name", name, true);
            if (disabled)
            {
                builder.MergeAttribute("disabled", "disabled");
            }

            var valueParameter = Convert.ToString(value, CultureInfo.CurrentCulture);
            builder.MergeAttribute("value", valueParameter);

            builder.GenerateId(name);

            var control = builder.ToString(TagRenderMode.SelfClosing);
            return MvcHtmlString.Create(control);
        }
    }
}