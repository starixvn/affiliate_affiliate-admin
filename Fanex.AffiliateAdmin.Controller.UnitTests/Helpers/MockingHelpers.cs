using Fanex.AffiliateAdmin.Core.Session;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Fanex.AffiliateAdmin.Controller.UnitTests.Helpers
{
    public static class MockingHelpers
    {
        public static void SetUpHttpContextSession(string key = "Fanex.AffiliateAdmin.Core.CurrentSession", object value = null)
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

        public static HttpRequestBase GetHttpRequest(string key = null, string value = null)
        {
            var mockHttpRequest = Substitute.For<HttpRequestBase>();
            //mockHttpRequest.ApplicationPath.Returns("/");
            //mockHttpRequest.UserHostAddress.Returns("::1");
            //mockHttpRequest.Url.Returns(new Uri(Constant.DefaultRootDomain));
            //mockHttpRequest.Browser.Browser.Returns(Constant.Browser);
            //mockHttpRequest[Arg.Compat.Any<string>()].Returns("");
            mockHttpRequest.ServerVariables.Returns(new NameValueCollection {
                [key] = value
            });

            return mockHttpRequest;
        }
    }
}
