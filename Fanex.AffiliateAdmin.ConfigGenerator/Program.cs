// --------------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.ConfigGenerator
{
    using System.IO;
    using System.Xml.Linq;

    /// <summary>
    /// Program class.
    /// </summary>
    public static class Program
    {
        private static void Main()
        {
            OperateXtest(OperationType.Add);

            ////OperateXtest(OperationType.Remove);
        }

        /// <summary>
        /// Operate on test store procedure.
        /// </summary>
        /// <param name="type">Operation type.</param>
        private static void OperateXtest(OperationType type)
        {
            const string XmlFile = @"..\..\XtestSps.xml";

            const string SpNameFile = @"..\..\SpName.tt";

            var document = new XDocument();

            if (!string.IsNullOrEmpty(XmlFile))
            {
                document = XDocument.Load(XmlFile);
            }

            var sps = document.Descendants("sp");

            var content = File.ReadAllText(SpNameFile);

            const string OldValueFormat = "SpName=\"{0}\"";

            const string XtestValueFormat = "SpName=\"{0}_xtest\"";

            foreach (var sp in sps)
            {
                var value = GetNodeValue(sp);

                switch (type)
                {
                    case OperationType.Add:
                        content = content.Replace(string.Format(OldValueFormat, value), string.Format(XtestValueFormat, value));
                        break;

                    case OperationType.Remove:
                        content = content.Replace(string.Format(XtestValueFormat, value), string.Format(OldValueFormat, value));
                        break;
                }
            }

            File.WriteAllText(SpNameFile, content);
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