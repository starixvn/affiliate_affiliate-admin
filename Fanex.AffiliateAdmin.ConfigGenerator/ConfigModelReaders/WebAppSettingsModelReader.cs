// --------------------------------------------------------------------------
// <copyright file="WebAppSettingsModelReader.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.ConfigGenerator.ConfigModelReaders
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Web AppSettings ModelReader.
    /// </summary>
    public class WebAppSettingsModelReader : ModelReader
    {
        /// <summary>
        /// Get settings of site.
        /// </summary>
        /// <returns>Setting list.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Dictionary<string, string> GetSettingsOfSite()
        {
            var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            var node = Xdocument.Descendants().FirstOrDefault();

            if (node == null)
            {
                return dict;
            }

            var elements = node.Elements();

            foreach (var element in elements)
            {
                if (IsNormalNode(element))
                {
                    var setting = GetSettingAtNormalNode(element);
                    dict.Add(setting.Key, setting.Value);
                }

                if (IsNodeBasedOnEnvironment(element))
                {
                    var setting = GetSettingAtNodeBasedOnEnvironment(element);
                    dict.Add(setting.Key, setting.Value);
                }
            }

            return dict;
        }
    }
}