using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Models.AffiliateInfo;
using Fanex.AffiliateAdmin.Services;
using NSubstitute;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests
{
    public class AdjustmentControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly ISystemSettings _systemSettings;
        private readonly IAdjustmentService _adjustmentService;

        public AdjustmentControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _adjustmentService = Substitute.For<IAdjustmentService>();
            _systemSettings = MockingHelpers.GetSystemSettings();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Index_GET_WhenCall_ReturnView(bool hasCookies)
        {
            //Arrange
            var httpCookieCollection = hasCookies ? new HttpCookieCollection() : Constants.HttpCookieCollection;
            var httpRequest = MockingHelpers.GetHttpRequest(httpCookieCollection: httpCookieCollection);

            _adjustmentService.GetAdjustmentBalance(Arg.Any<AdjustmentCriteria>()).Returns(new Adjustment());

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);

            var result = controller.Index() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model);
        }

        [Fact]
        public void Index_POST_WhenCall_ReturnView()
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();
            var httpRequest = MockingHelpers.GetHttpRequest("REMOTE_ADDR", "::1", new HttpCookieCollection());

            _adjustmentService.GetAdjustmentBalance(Arg.Any<AdjustmentCriteria>()).Returns(new Adjustment());

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);

            var result = controller.Index(new AdjustmentViewModel()) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model);
        }

        private AdjustmentController CreateController()
            => new AdjustmentController(_appSetting, _systemSettings, _adjustmentService);
    }
}
