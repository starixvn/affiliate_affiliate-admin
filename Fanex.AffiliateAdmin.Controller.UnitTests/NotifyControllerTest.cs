using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Core.Session;
using NSubstitute;
using ONEbook.UM.Models;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests
{
    public class NotifyControllerTest
    {
        private readonly IUnderMaintenanceService _maintenanceService;
        private readonly IFormsAuthenticationWrapper _formsAuthenticationWrapper;
        public NotifyControllerTest()
        {
            _maintenanceService = Substitute.For<IUnderMaintenanceService>();
            _formsAuthenticationWrapper = Substitute.For<IFormsAuthenticationWrapper>();
        }

        [Fact]
        public void Index_WhenCall_RedirectResult()
        {
            //Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);

            var urlHelperMock = Substitute.For<UrlHelper>();
            urlHelperMock.Content(Arg.Any<string>()).Returns("/");

            //Action
            var controller = CreateController();
            controller.Url = urlHelperMock;
            var result = controller.Index() as RedirectResult;

            //Assert
            Assert.True(result.Permanent);
            Assert.Equal("/", result.Url);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Index_WhenCall_ReturnCorrectView(bool isNeedLogOut)
        {
            //Arrange
            _maintenanceService.IsUnderMaintenance().Returns(true);
            _maintenanceService.GetUnderMaintenanceTime().Returns(new UnderMaintenanceTime { From = "13:00", To = "15:00" });

            var currentSession = isNeedLogOut ? Substitute.For<ICurrentSession>() : null;
            MockingHelpers.SetUpHttpContextSession(currentSession);

            //Action
            var controller = CreateController();
            var result = controller.Index() as ViewResult;
            var viewBag = result.ViewBag;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.Equal("13:00", viewBag.StartTime);
            Assert.Equal("15:00", viewBag.EndTime);
        }

        [Theory]
        [InlineData(100, "Token was expired")]
        [InlineData(200, "Licensee is not existed")]
        [InlineData(300, "Token is invalid")]
        [InlineData(400, "Direct Access to this website is restricted. Please contact Administrator for further enquiries.")]
        [InlineData(600, "Licensee does not offered currency")]
        [InlineData(700, "You are not allowed to access this page !!")]
        [InlineData(123, "Who let you here ?")]
        public void OneTokenRestriction_WhenCall_ReturnCorrectView(int code, string expectedMessage)
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.OneTokenRestriction(code) as ViewResult;
            var viewBag = result.ViewBag;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.Equal(expectedMessage, viewBag.Message);
        }

        [Fact]
        public void Error_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.Error() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Fact]
        public void PageNotFound_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.PageNotFound() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Fact]
        public void AccessDenied_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.AccessDenied() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        private NotifyController CreateController()
            => new NotifyController(_maintenanceService, _formsAuthenticationWrapper);
    }
}
