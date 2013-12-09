// --------------------------------------------------------------------------
// <copyright file="AttributesExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.22">Created</change>
// </history>

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