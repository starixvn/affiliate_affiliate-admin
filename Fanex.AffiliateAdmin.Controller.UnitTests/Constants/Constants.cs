namespace Fanex.AffiliateAdmin.Controller.UnitTests
{
    using System.Web;

    public static class Constants
    {
        public static HttpCookieCollection HttpCookieCollection
        {
            get
            {
                var affInfo = new HttpCookie("AffInfo");
                affInfo["AffId"] = "123";
                affInfo["Status"] = "Approved";
                affInfo["AffName"] = "Test123";
                affInfo["CurrencyId"] = "12";
                affInfo["CurrencyCode"] = "USD";

                return new HttpCookieCollection {
                    affInfo
                };
            }
        }
    }
}
