// --------------------------------------------------------------------------
// <copyright file="SiteManager.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Site manager.
    /// </summary>
    public class SiteManager
    {
        private XDocument _document;

        private string _globalWebConfigModel;

        public string XmlFile { get; set; }

        /// <summary>
        /// Get Transform Status.
        /// </summary>
        /// <returns>Transform Status.</returns>
        public bool GetTransformStatus()
        {
           
            var node = _document.Descendants().FirstOrDefault();

            if (node == null)
            {
                return false;
            }

            return GetNodeValue(node.Element("transform")).Equals("1", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get global web configuration model.
        /// </summary>
        /// <returns>Model path.</returns>
        public string GetGlobalWebConfigModel()
        {
            if (string.IsNullOrEmpty(_globalWebConfigModel))
            {
                var node = _document.Descendants().FirstOrDefault();

                if (node == null)
                {
                    return string.Empty;
                }

                _globalWebConfigModel = GetNodeValue(node.Element("webconfigmodel"));
            }

            return _globalWebConfigModel;
        }

        /// <summary>
        /// Get supported environment list.
        /// </summary>
        /// <returns>Environment list.</returns>
        public Dictionary<string, bool> GetSupportedEnvironmentList()
        {
            var environments = new Dictionary<string, bool>();
            var node = _document.Descendants("environments").FirstOrDefault();

            if (node == null)
            {
                return environments;
            }

            var elements = node.Elements();

            foreach (var environmentElement in elements)
            {
                var environment = GetNodeValue(environmentElement);
                var enable = GetAttributeValue(environmentElement.Attribute("enable")).Equals("1", StringComparison.OrdinalIgnoreCase);

                environments.Add(environment, enable);
            }

            return environments;
        }

        /// <summary>
        /// Get supported site list.
        /// </summary>
        /// <returns>Site list.</returns>
        public IList<SiteInfo> GetSupportedSiteList()
        {
            var siteList = new List<SiteInfo>();
            var node = _document.Descendants("sites").FirstOrDefault();

            if (node == null)
            {
                return siteList;
            }

            var elements = node.Elements();

            foreach (var siteElement in elements)
            {
                var site = new SiteInfo
                    {
                        Name = GetNodeValue(siteElement.Element("name")),
                        Status = GetNodeValue(siteElement.Element("status")).Equals("1", StringComparison.OrdinalIgnoreCase),
                        Environment = GetNodeValue(siteElement.Element("environment")),
                        WebConfigModel = GetNodeValue(siteElement.Element("webconfigmodel")),
                        WebAppSettingModel = GetNodeValue(siteElement.Element("webappsettingmodel")),
                        WebConnectionStringModel = GetNodeValue(siteElement.Element("webconnectionstringmodel"))
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