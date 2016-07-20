namespace ConfigGenerator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Web config model reader.
    /// </summary>
    public class WebConfigModelReader : ModelReader
    {
        /// <summary>
        /// Get settings of node.
        /// </summary>
        /// <param name="xname">Element name.</param>
        /// <returns>Settings of node.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", Justification = "Accept this method")]
        public Dictionary<string, string> GetSettingsOfNode(string xname)
        {
            var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            var node = Xdocument.Descendants(xname).FirstOrDefault();

            if (node == null)
            {
                return dict;
            }

            if (IsNodeBasedOnEnvironment(node))
            {
                var setting = GetSettingAtNodeBasedOnEnvironment(node);

                if (!string.IsNullOrEmpty(setting.Key))
                {
                    dict.Add(setting.Key, setting.Value);
                }

                return dict;
            }

            GetSettingsOfNode(node, dict);

            return dict;
        }

        /// <summary>
        /// Get settings of node.
        /// </summary>
        /// <param name="node">Element node.</param>
        /// <param name="dict">Setting dictionary.</param>
        protected void GetSettingsOfNode(XElement node, Dictionary<string, string> dict)
        {
            if (IsNodeBasedOnEnvironment(node))
            {
                var setting = GetSettingAtNodeBasedOnEnvironment(node);
                if (!string.IsNullOrEmpty(setting.Key))
                {
                    dict.Add(setting.Key, setting.Value);
                }
            }

            var elements = node.Elements();

            foreach (var element in elements)
            {
                GetSettingsOfNode(element, dict);
            }
        }
    }
}