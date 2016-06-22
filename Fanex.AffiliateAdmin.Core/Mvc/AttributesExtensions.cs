namespace Fanex.AffiliateAdmin.Core.Mvc
{
    using System.Web.Routing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class AttributesExtensions
    {
        public static RouteValueDictionary DisabledIf(this object htmlAttributes, bool disabled)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            if (disabled)
            {
                attributes["disabled"] = "disabled";
            }

            return attributes;
        }

        public static RouteValueDictionary WithAttrIf(this object htmlAttributes, string attrName, bool disabled)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            if (disabled)
            {
                attributes[attrName] = attrName;
            }

            return attributes;
        }
    }
}