using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.AffiliateAdmin.ModelReaders
{
    public class WebAppSettingsModelReader : ModelReader
    {
         public Dictionary<string, string> GetSettingsOfSite()
         {
             var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

             var node = XDocument.Descendants().FirstOrDefault();

             if (node == null)
                 return dict;

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