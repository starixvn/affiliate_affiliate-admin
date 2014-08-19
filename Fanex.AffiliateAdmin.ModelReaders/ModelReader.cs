using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Fanex.AffiliateAdmin.ModelReaders
{
    public class ModelReader
    {
        protected XDocument XDocument;

        public string XmlFile { get; set; }

        public string Environment { get; set; }

        public void LoadXml()
        {
            if (!string.IsNullOrEmpty(XmlFile))
            {
                XDocument = XDocument.Load(XmlFile);
            }
        }

        protected bool IsNodeBasedOnEnvironment(XElement element)
        {
            var child = element.Element("local");

            return child != null;
        }

        protected bool IsNormalNode(XElement element)
        {
            var children = element.Elements();

            return !children.Any();
        }

        public virtual KeyValuePair<string, string> GetSettingAtNodeBasedOnEnvironment(XElement element)
        {
            var key = element.Name.LocalName;
            var childElement = element.Element(Environment);

            if (childElement == null)
            {
                return new KeyValuePair<string, string>();
            }

            var value = childElement.Value;
            Console.WriteLine("{0} = {1}", key, value);

            return new KeyValuePair<string, string>(key, value);
        }

        public virtual KeyValuePair<string, string> GetSettingAtNormalNode(XElement element)
        {
            var key = element.Name.LocalName;
            var value = element.Value;
            Console.WriteLine("{0} = {1}", key, value);

            return new KeyValuePair<string, string>(key, value);
        }
    }
}