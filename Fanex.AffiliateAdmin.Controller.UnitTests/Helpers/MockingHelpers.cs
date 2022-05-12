using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Session;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Fanex.AffiliateAdmin.Controller.UnitTests.Helpers
{
    public static class MockingHelpers
    {
        public static void SetUpEmptyHttpContextSession()
        {
            SetUpHttpContextSession(Substitute.For<ICurrentSession>());
        }

        public static void SetUpHttpContextSession(object value = null, string key = "Fanex.AffiliateAdmin.Core.CurrentSession")
        {
            var request = new HttpRequest(null, "http://abco.com", null);
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection {
                [key] = value
            },
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            SessionStateUtility.AddHttpSessionStateToContext(HttpContext.Current, sessionContainer);
        }

        public static HttpRequestBase GetHttpRequest(string key = null, string value = null, HttpCookieCollection httpCookieCollection = null)
        {
            var mockHttpRequest = Substitute.For<HttpRequestBase>();
            mockHttpRequest.UserHostName.Returns("test.com");
            //mockHttpRequest.ApplicationPath.Returns("/");
            //mockHttpRequest.UserHostAddress.Returns("::1");
            //mockHttpRequest.Url.Returns(new Uri(Constant.DefaultRootDomain));
            //mockHttpRequest.Browser.Browser.Returns(Constant.Browser);
            //mockHttpRequest[Arg.Compat.Any<string>()].Returns("");
            mockHttpRequest.ServerVariables.Returns(new NameValueCollection {
                [key] = value
            });
            mockHttpRequest.Cookies.Returns(httpCookieCollection ?? Constants.HttpCookieCollection);

            return mockHttpRequest;
        }

        public static HttpResponseBase GetHttpResponse(HttpCookieCollection httpCookieCollection = null)
        {
            var httpResponse = Substitute.For<HttpResponseBase>();
            httpResponse.Cookies.Returns(httpCookieCollection ?? Constants.HttpCookieCollection);

            return httpResponse;
        }

        public static ControllerContext GetControllerContext(HttpRequestBase httpRequest = null, HttpResponseBase httpResponse = null)
        {
            var controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext.Request.Returns(httpRequest);
            controllerContext.HttpContext.Response.Returns(httpResponse);

            return controllerContext;
        }

        public static ISystemSettings GetSystemSettings(ISystemSettings systemSettings = null)
        {
            systemSettings = systemSettings ?? Substitute.For<ISystemSettings>();
            systemSettings.Products.Returns(new List<Product>
            {
                new Product {
                    IsEnabled = true,
                    ProductID = 123,
                    ProductName = "AG Casino"
                },
                new Product {
                    IsEnabled = true,
                    ProductID = 234,
                    ProductName = "ABC"
                }
            });
            systemSettings.AffLanguages.Returns(new List<Language>
            {
                new Language {
                    LanguageID = "en-US",
                    LanguageName = "English"
                }
            });
            systemSettings.PlayerCurrencies.Returns(new List<Currency>
            {
                new Currency {
                    CurrencyID = 123,
                    CurrencyCode = "USD"
                }
            });
            systemSettings.GetCommGroups(Arg.Any<CommGroupCurrencyCriteria>()).Returns(new List<CommGroup>
            {
                new CommGroup {
                    GroupID = 123,
                    GroupName = "NoCommission"
                }
            });
            systemSettings.AffTypes.Returns(new List<AffType>
            {
                new AffType {
                    TypeID = 1,
                    Name = "EURO"
                }
            });
            systemSettings.BaseExchange.Returns(
                new Exchange {
                    Code = "USD",
                    ExchangeID = 123
                }
            );

            return systemSettings;
        }
    }
}
