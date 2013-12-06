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

namespace Fanex.NAP.Settlement.Web.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using System.Xml.Linq;
    using Fanex.Logging;

    /// <summary>
    ///     Caching configuration file, implemented by internal constructor + safe singleton + lazy load pattern.
    /// </summary>
    public static class SportViewSettingFactory
    {
        /// <summary>
        /// Sport View SettingMapper.
        /// </summary>
        private static IDictionary<int, SportViewSetting> sportViewSettingMapper = new Dictionary<int, SportViewSetting>();

        /// <summary>
        /// Initializes static members of the <see cref="SportViewSettingFactory" /> class.
        /// </summary>
        static SportViewSettingFactory()
        {
            try
            {
                var document =
                    XDocument.Load(
                        HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SportTypeSetting"]));
                var root = document.Root;

                if (root != null)
                {
                    foreach (var elementSportTypeMapping in root.Elements())
                    {
                        foreach (var sportTypeElement in elementSportTypeMapping.Elements())
                        {
                            var sportViewSetting = new SportViewSetting
                                {
                                    SportType = Convert.ToInt32(sportTypeElement.Attribute("Id").Value),
                                    GroupSport = Convert.ToInt32(elementSportTypeMapping.Attribute("GroupSport").Value),
                                    Template = GetXmlAttributeString(elementSportTypeMapping.Attribute("Template")),
                                    DirectlyTemplate =
                                        GetXmlAttributeString(elementSportTypeMapping.Attribute("DirectlyTemplate")),
                                    IsCheckBoxCalcMp =
                                        Convert.ToBoolean(elementSportTypeMapping.Attribute("IsCheckBoxCalcMP").Value),
                                    Colspan = elementSportTypeMapping.Attribute("Colspan") != null
                                                  ? Convert.ToInt32(elementSportTypeMapping.Attribute("Colspan").Value)
                                                  : 0
                                };

                            sportViewSettingMapper.Add(sportViewSetting.SportType, sportViewSetting);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogService.Error("Could not load SportTypeSetting configruation file.", exception);
                throw;
            }
        }

        #region Factory

        /// <summary>
        /// Function Create.
        /// </summary>
        /// <param name="sportType">Sport type.</param>
        /// <returns>Sport view setting.</returns>
        public static SportViewSetting Create(int sportType)
        {
            return sportViewSettingMapper.ContainsKey(sportType)
                       ? sportViewSettingMapper[sportType]
                       : new SportViewSetting
                           {
                               Template = "Soccer",
                               DirectlyTemplate = "DirectScore/Soccer",
                               IsCheckBoxCalcMp = true,
                               Colspan = 8
                           };
        }

        #endregion Factory

        /// <summary>
        /// Check exist attribute on XML Mapping.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns>Return string.</returns>
        private static string GetXmlAttributeString(XAttribute attributeName)
        {
            return attributeName != null ? attributeName.Value : string.Empty;
        }
    }
}