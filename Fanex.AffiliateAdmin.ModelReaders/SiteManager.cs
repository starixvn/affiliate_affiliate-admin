using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Fanex.AffiliateAdmin.ModelReaders
{
    public class SiteManager
    {
        private string _globalWebConfigModel;

        protected XDocument XDocument;

        public string XmlFile { get; set; }

        public void LoadXml()
        {
            if (!string.IsNullOrEmpty(XmlFile))
            {
                XDocument = XDocument.Load(XmlFile);
            }
        }

        public string GetGlobalWebConfigModel()
        {
            if (string.IsNullOrEmpty(_globalWebConfigModel))
            {
                var node = XDocument.Descendants().FirstOrDefault();

                if (node == null)
                    return string.Empty;

                _globalWebConfigModel = GetNodeValue(node.Element("webconfigmodel"));
            }

            return _globalWebConfigModel;
        }

        public Dictionary<string, bool> GetSupportedEnvironmentList()
        {
            var environments = new Dictionary<string, bool>();
            var node = XDocument.Descendants("environments").FirstOrDefault();

            if (node == null)
                return environments;

            var elements = node.Elements();

            foreach (var environmentElement in elements)
            {
                var environment = GetNodeValue(environmentElement);
                var enable = GetAttributeValue(environmentElement.Attribute("enable")).Equals("1", StringComparison.OrdinalIgnoreCase);
                
                environments.Add(environment, enable);
            }

            return environments;
        }

        public IList<SiteInfo> GetSupportedSiteList()
        {
            var siteList = new List<SiteInfo>();
            var node = XDocument.Descendants("sites").FirstOrDefault();

            if (node == null)
                return siteList;

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

        private string GetNodeValue(XElement element)
        {
            if (element != null)
            {
                var xValue = element.Value;
                Console.WriteLine("{0} : {1}", element.Name.LocalName, xValue);

                return xValue;
            }

            return string.Empty;
        }

        private string GetAttributeValue(XAttribute attribute)
        {
            if (attribute != null)
            {

                var enable = attribute.Value;

                Console.WriteLine("{0} : {1}", attribute.Name.LocalName, enable);

                return enable;
            }

            return string.Empty;
        }
    }
}