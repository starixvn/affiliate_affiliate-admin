namespace Fanex.AffiliateAdmin.Controller.Helpers
{
    using System;
    using System.Web;

    public static class AffiliateInfoCookieHelper
    {
        public static void AddAffiliateInfoToCookie(HttpContextBase context, string userName)
        {
            var cookie = new HttpCookie("AffInfo");
            cookie["AffName"] = userName;
            cookie.Expires = DateTime.Now.AddDays(1d);
            context.Response.Cookies.Add(cookie);
        }

        public static void DeleteAffiliateInfoCookie(HttpContextBase context)
        {
            if (context.Request.Cookies["AffInfo"] != null)
            {
                context.Response.Cookies["AffInfo"].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}