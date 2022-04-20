using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Core.Session;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Models;
using Fanex.AffiliateAdmin.Services;
using NSubstitute;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests
{
    public class AdjustmentControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly IAdjustmentService _adjustmentService;
        private readonly ISystemSettingService _systemSettingService;

        public AdjustmentControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _adjustmentService = Substitute.For<IAdjustmentService>();
            _systemSettingService = Substitute.For<ISystemSettingService>();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Index_GET_WhenCall_ReturnView(bool hasCookies)
        {
            //Arrange
            var httpCookieCollection = new HttpCookieCollection();

            if (hasCookies)
            {
                var affInfo = new HttpCookie("AffInfo");
                affInfo["AffId"] = "123";
                affInfo["Status"] = "Approved";
                affInfo["AffName"] = "Test123";
                affInfo["CurrencyId"] = "12";
                affInfo["CurrencyCode"] = "USD";

                httpCookieCollection = new HttpCookieCollection { affInfo };
            }

            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Cookies.Returns(httpCookieCollection);
            var controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext.Request.Returns(httpRequest);

            _adjustmentService.GetAdjustmentBalance(Arg.Any<AdjustmentCriteria>()).Returns(new Adjustment());

            //Action
            var controller = CreateController();
            controller.ControllerContext = controllerContext;

            var result = controller.Index() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model);
        }

        [Fact]
        public void Index_POST_WhenCall_ReturnView()
        {
            //Arrange
            MockingHelpers.SetUpHttpContextSession(value: Substitute.For<ICurrentSession>());

            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Cookies.Returns(new HttpCookieCollection());
            httpRequest.ServerVariables.Returns(new NameValueCollection { ["REMOTE_ADDR"] = "::1" });
            var controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext.Request.Returns(httpRequest);

            _adjustmentService.GetAdjustmentBalance(Arg.Any<AdjustmentCriteria>()).Returns(new Adjustment());

            //Action
            var controller = CreateController();
            controller.ControllerContext = controllerContext;

            var result = controller.Index(new AdjustmentViewModel()) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model);
        }

        private AdjustmentController CreateController()
            => new AdjustmentController(_appSetting, _adjustmentService, _systemSettingService);
    }
}
