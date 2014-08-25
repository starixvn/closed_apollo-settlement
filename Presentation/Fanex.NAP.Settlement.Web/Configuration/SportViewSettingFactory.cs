// -----------------------------------------------------------------------
// <copyright file="SportViewSettingFactory.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Auto get template content and header
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.14">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using System.Xml.Linq;
    using Fanex.NAP.Settlement.Core.Constants;

    /// <summary>
    ///     Caching configuration file, implemented by internal constructor + safe singleton + lazy load pattern.
    /// </summary>
    public static class SportViewSettingFactory
    {
        static SportViewSettingFactory()
        {
            SportViewSettingMapper = new Dictionary<int, SportViewSetting>();
            var document =
                XDocument.Load(
                    HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SportTypeSetting"]));
            var root = document.Root;

            if (root == null)
            {
                return;
            }

            foreach (var elementSportTypeMapping in root.Elements())
            {
                foreach (var sportTypeElement in elementSportTypeMapping.Elements())
                {
                    var sportViewSetting = SetValue(Convert.ToByte(sportTypeElement.Attribute("Id").Value), elementSportTypeMapping);

                    SportViewSettingMapper.Add(sportViewSetting.SportType, sportViewSetting);
                }
            }
        }

        private static IDictionary<int, SportViewSetting> SportViewSettingMapper { get; set; }

        #region Factory

        /// <summary>
        /// Get Sport View setting by sport type.
        /// </summary>
        /// <param name="sportType">Sport type value.</param>
        /// <returns>SportViewSetting object.</returns>
        public static SportViewSetting Create(byte sportType)
        {
            return SportViewSettingMapper.ContainsKey(sportType)
                       ? SportViewSettingMapper[sportType]
                       : SportViewSettingMapper[SportType.OtherSports];
        }

        #endregion Factory

        private static string GetXmlAttributeString(XAttribute attributeName)
        {
            return attributeName != null ? attributeName.Value : string.Empty;
        }

        /// <summary>
        /// Set value to view Setting.
        /// </summary>
        /// <param name="sportType">Sport type.</param>
        /// <param name="element">Element value.</param>
        /// <returns>New Setting.</returns>
        private static SportViewSetting SetValue(byte sportType, XElement element)
        {
            var sportViewSetting = new SportViewSetting
            {
                SportType = sportType,
                GroupSport = Convert.ToByte(element.Attribute("GroupSport").Value),
                Template = GetXmlAttributeString(element.Attribute("Template")),
                DirectlyTemplate = GetXmlAttributeString(element.Attribute("DirectlyTemplate")),
                Colspan = element.Attribute("Colspan") != null ? Convert.ToInt32(element.Attribute("Colspan").Value) : 0
            };
            return sportViewSetting;
        }
    }
}