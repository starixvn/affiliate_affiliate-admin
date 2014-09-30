using System;
using System.IO;
using System.Xml.Linq;

namespace Fanex.AffiliateAdmin.ConfigGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            AddXtest();

            //RemoveXtest();
        }

        private static void RemoveXtest()
        {
            const string xmlFile = @"..\..\XtestSps.xml";
            const string spNameFile = @"..\..\SpName.tt";

            var document = new XDocument();

            if (!string.IsNullOrEmpty(xmlFile))
            {
                document = XDocument.Load(xmlFile);
            }

            var sps = document.Descendants("sp");

            var spName = File.ReadAllText(spNameFile);

            const string oldValueFormat = "SpName=\"{0}\"";

            const string xtestValueFormat = "SpName=\"{0}_xtest\"";

            foreach (var sp in sps)
            {
                var value = GetNodeValue(sp);
                spName = spName.Replace(string.Format(xtestValueFormat, value), string.Format(oldValueFormat, value));
            }

            File.WriteAllText(spNameFile, spName);
        }

        private static void AddXtest()
        {
            const string xmlFile = @"..\..\XtestSps.xml";
            const string spNameFile = @"..\..\SpName.tt";

            var document = new XDocument();

            if (!string.IsNullOrEmpty(xmlFile))
            {
                document = XDocument.Load(xmlFile);
            }

            var sps = document.Descendants("sp");

            var spName = File.ReadAllText(spNameFile);
            
            const string oldValueFormat = "SpName=\"{0}\"";

            const string xtestValueFormat = "SpName=\"{0}_xtest\"";

            foreach (var sp in sps)
            {
                var value = GetNodeValue(sp);
                spName = spName.Replace(string.Format(oldValueFormat, value), string.Format(xtestValueFormat, value));
            }

            File.WriteAllText(spNameFile, spName);
        }

        private static string GetNodeValue(XElement element)
        {
            if (element != null)
            {
                return element.Value;
            }

            return string.Empty;
        }
    }
}