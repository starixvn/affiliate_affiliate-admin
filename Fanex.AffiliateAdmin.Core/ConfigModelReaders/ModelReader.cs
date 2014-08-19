// --------------------------------------------------------------------------
// <copyright file="ModelReader.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.ConfigModelReaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Model reader for t4 template.
    /// </summary>
    public class ModelReader
    {
        protected XDocument xdocument;

        public string Environment { get; set; }

        public string XmlFile { get; set; }

        /// <summary>
        /// Get setting at node based on environment.
        /// </summary>
        /// <param name="element">Element input.</param>
        /// <returns>Key value of setting.</returns>
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

        public void LoadXml()
        {
            if (!string.IsNullOrEmpty(XmlFile))
            {
                xdocument = XDocument.Load(XmlFile);
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
    }
}