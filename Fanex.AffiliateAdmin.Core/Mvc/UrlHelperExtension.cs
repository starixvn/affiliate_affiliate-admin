using System.Configuration;
using System.Web.Mvc;

namespace Fanex.AffiliateAdmin.Core.Mvc
{
    public static class UrlHelperExtension
    {
        public static string RenderLink(this UrlHelper urlHelper, string virtualPath)
        {
            var jsVersion = ConfigurationManager.AppSettings["JsVersion"];
            return urlHelper.Content(virtualPath) + "?v=" + jsVersion;
        }
    }
}
