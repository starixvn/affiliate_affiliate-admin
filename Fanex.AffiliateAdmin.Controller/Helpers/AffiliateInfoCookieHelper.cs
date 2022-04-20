namespace Fanex.AffiliateAdmin.Controller.Helpers
{
    using Fanex.AffiliateAdmin.DTO;
    using System;
    using System.Web;

    public static class AffiliateInfoCookieHelper
    {
        public static void SaveAffiliate(HttpContextBase context, Affiliate affiliate)
        {
            var cookie = new HttpCookie("AffInfo");
            cookie["AffId"] = affiliate.AffId.ToString();
            cookie["Status"] = affiliate.Status;
            cookie["AffName"] = affiliate.UserName;
            cookie["CurrencyId"] = affiliate.Currency.ToString();
            cookie["CurrencyCode"] = affiliate.CurrencyCode;
            cookie.Expires = DateTime.Now.AddDays(1d);
            context.Response.Cookies.Add(cookie);
        }

        public static void DeleteAffiliate(HttpContextBase context)
        {
            if (context.Request.Cookies["AffInfo"] != null)
            {
                context.Response.Cookies["AffInfo"].Expires = DateTime.Now.AddDays(-1);
            }
        }

        public static Affiliate GetAffiliate(HttpContextBase context)
        {
            var cookie = context.Request.Cookies["AffInfo"];
            if (cookie == null)
            {
                return new Affiliate();
            }

            return new Affiliate
            {
                AffId = int.Parse(cookie["AffId"]),
                Status = cookie["Status"],
                UserName = cookie["AffName"],
                Currency = short.Parse(cookie["CurrencyId"]),
                CurrencyCode = cookie["CurrencyCode"]
            };
        }
    }
}