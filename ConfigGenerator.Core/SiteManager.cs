// --------------------------------------------------------------------------
// <copyright file="SiteManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.8.19">Created</change>
// </history>

namespace ConfigGenerator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Site manager.
    /// </summary>
    public class SiteManager
    {
        private XDocument _document;

        public string XmlFile { get; set; }

        /// <summary>
        /// Get supported site list.
        /// </summary>
        /// <returns>Site list.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Accepted")]
        public IList<Licensee> GetSupportedLicenseeList()
        {
            var siteList = new List<Licensee>();
            var node = _document.Descendants("sites").FirstOrDefault();

            if (node == null)
            {
                return siteList;
            }

            var elements = node.Elements();

            foreach (var siteElement in elements)
            {
                var site = new Licensee
                    {
                        Name = GetNodeValue(siteElement.Element("name")),
                        Status = GetNodeValue(siteElement.Element("status")).Equals("1", StringComparison.OrdinalIgnoreCase),
                    };

                siteList.Add(site);
            }

            return siteList;
        }

        /// <summary>
        /// Load xml file.
        /// </summary>
        public void LoadXml()
        {
            if (!string.IsNullOrEmpty(XmlFile))
            {
                _document = XDocument.Load(XmlFile);
            }
        }

        /// <summary>
        /// Get attribute value.
        /// </summary>
        /// <param name="attribute">Xml attribute.</param>
        /// <returns>Attribute value.</returns>
        private string GetAttributeValue(XAttribute attribute)
        {
            if (attribute != null)
            {
                var enable = attribute.Value;

                return enable;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get node value.
        /// </summary>
        /// <param name="element">Xml element.</param>
        /// <returns>Node value.</returns>
        private string GetNodeValue(XElement element)
        {
            if (element != null)
            {
                return element.Value;
            }

            return string.Empty;
        }
    }
}