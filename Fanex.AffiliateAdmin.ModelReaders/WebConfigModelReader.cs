using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Fanex.AffiliateAdmin.ModelReaders
{
    public class WebConfigModelReader : ModelReader
    {
         public Dictionary<string, string> GetSettingsOfNode(string xName)
         {
             var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

             var node = XDocument.Descendants(xName).FirstOrDefault();

             if (node == null)
                 return dict;

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