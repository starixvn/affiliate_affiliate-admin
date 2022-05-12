using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Core.Session;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Models;
using Fanex.AffiliateAdmin.Services;
using NSubstitute;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests
{
    public class AccountControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly ISystemSettings _systemSettings;
        private readonly IFormsAuthenticationWrapper _formsAuthenticationWrapper;

        public AccountControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _authService = Substitute.For<IAuthService>();
            _accountService = Substitute.For<IAccountService>();
            _systemSettings = MockingHelpers.GetSystemSettings();
            _formsAuthenticationWrapper = Substitute.For<IFormsAuthenticationWrapper>();
        }

        [Fact]
        public void ChangeLanguage_WhenCall_ReturnCorrectData()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.ChangeLanguage("vi-VN") as JsonResult;

            //Assert
            Assert.Equal("", result.Data);
            Assert.Equal("vi-VN", Convert.ToString(HttpContext.Current.Session["AffiliateLanguage"]));
        }

        [Fact]
        public void Login_GET_WhenCall_ReturnCorrectView()
        {
            //Arrange
            _appSetting.IsProduction.Returns(false);

            //Action
            var controller = CreateController();
            var result = controller.Login() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model as LoginViewModel);
        }

        [Theory]
        [InlineData("HTTP_X_FORWARDED_FOR", "127.0.0.1", "::1")]
        [InlineData("HTTP_X_FORWARDED_FOR", "127.0.0.1,127.0.0.2", "::1")]
        [InlineData("REMOTE_ADDR", "::1", "127.0.0.1")]
        public void Login_GET_WhenCallWithIsProduction_RedirectResult(string key, string value, string companyIPRange)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest(key, value);

            _appSetting.IsProduction.Returns(true);
            _systemSettings.SystemName.Returns("Test");
            _appSetting.GetValue("CompanyIPRange").Returns(companyIPRange);

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);

            var result = controller.Login() as RedirectResult;

            //Assert
            Assert.Equal("~/Notify/AccessDenied", result.Url);
        }

        [Fact]
        public void Login_POST_WhenCall_ReturnNull()
        {
            //Arrange

            //Action
            var controller = CreateController();
            controller.ModelState.AddModelError("Error1", "error");

            var result = controller.Login("", "", "") as JsonResult;

            //Assert
            Assert.Null(result.Data as ResultViewModel);
        }

        [Theory]
        [InlineData(103, "", "Account closed. Please contact your administrator!")]
        [InlineData(103, "errorMessage", "errorMessage")]
        [InlineData(0, "", "Updated successfully.")]
        public void Login_POST_WhenCall_ReturnCorrectData(int errCode, string message, string expectedMessage)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest("REMOTE_ADDR", "::1");

            _authService.Login(Arg.Any<LoginCriteria>(), out Arg.Any<Account>(), Arg.Any<string>()).Returns(prs => {
                prs[1] = new Account();
                return new ResultViewModel { ErrorCode = errCode, ErrorMessage = message };
            });

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.Login("", "", "") as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(expectedMessage, data.ErrorMessage);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Login_POST_WhenCall_LoginSuccess(bool isNull)
        {
            //Arrange
            var httpCookieCollection = isNull ? new HttpCookieCollection() : Constants.HttpCookieCollection;

            var httpRequest = MockingHelpers.GetHttpRequest("REMOTE_ADDR", "::1", httpCookieCollection);
            var httpResponse = MockingHelpers.GetHttpResponse(httpCookieCollection);

            _authService.Login(Arg.Any<LoginCriteria>(), out Arg.Any<Account>(), Arg.Any<string>()).Returns(prs => {
                prs[1] = new Account { CustID = 123 };
                return new ResultViewModel();
            });
            _authService.CreateInternalSession(Arg.Any<Account>(), Arg.Any<int>()).Returns(Substitute.For<ICurrentSession>());

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest, httpResponse);
            var result = controller.Login("", "", "") as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.True(data.IsSuccess);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("test", "salt")]
        public void GetUserSalt_WhenCall_ReturnCorrectData(string userName, string expectedSalt)
        {
            //Arrange
            _authService.GetByUsername(Arg.Any<string>()).Returns(string.IsNullOrEmpty(userName) ? null : new Account { UserSalt = expectedSalt });

            //Action
            var controller = CreateController();
            var result = controller.GetUserSalt(userName);

            //Assert
            Assert.Equal(expectedSalt, result);
        }

        [Fact]
        public void ChangePassword_GET_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.ChangePassword() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model as AccountPasswordViewModel);
        }

        [Fact]
        public void ChangePassword_POST_WhenCallWithInvalidModelState_ReturnNUll()
        {
            //Arrange

            //Action
            var controller = CreateController();
            controller.ModelState.AddModelError("Error01", "error");

            var result = controller.ChangePassword(new AccountPasswordViewModel()) as ViewResult;

            //Assert
            Assert.Null(result.ViewBag.Result as ResultViewModel);
        }

        [Fact]
        public void ChangePassword_POST_WhenCallWithNewSameOldPassword_ReturnError()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.ChangePassword(new AccountPasswordViewModel { NewPassword = "123abc", OldPassword = "123abc" }) as ViewResult;
            var resultViewModel = result.ViewBag.Result as ResultViewModel;
            //Assert
            Assert.Equal(16, resultViewModel.ErrorCode);
            Assert.Equal("New password must be different with old password!", resultViewModel.ErrorMessage);
        }

        [Fact]
        public void ChangePassword_POST_WhenCall_ChangePasswordSuccessfully()
        {
            //Arrange
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.Username.Returns("test");
            currentSession.CustID.Returns(123);

            _authService.GetByUsername(Arg.Any<string>()).Returns(new Account { UserSalt = "UserSalt" });

            //Action
            var controller = CreateController(currentSession);
            var result = controller.ChangePassword(new AccountPasswordViewModel { NewPassword = "123abc", OldPassword = "123xyz" }) as ViewResult;
            var resultViewModel = result.ViewBag.Result as ResultViewModel;
            //Assert
            Assert.True(resultViewModel.IsSuccess);
            Assert.Equal("Updated successfully.", resultViewModel.ErrorMessage);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Logout_WhenCall_LogoutSuccessfully(bool hasSession)
        {
            //Arrange
            var currentSession = hasSession ? Substitute.For<ICurrentSession>() : null;
            var httpRequest = MockingHelpers.GetHttpRequest();
            var httpResponse = MockingHelpers.GetHttpResponse();

            _formsAuthenticationWrapper.LoginUrl.Returns("~/Login");

            //Action
            var controller = CreateController(currentSession);
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest, httpResponse);
            var result = controller.Logout() as RedirectResult;

            //Assert
            Assert.Equal("~/Login", result.Url);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        public void ReCheckIn_WhenCallWithExpired_KickedOut(bool isLocalTest, bool hasSession, bool isKickedOut)
        {
            //Arrange
            var currentSession = hasSession ? Substitute.For<ICurrentSession>() : null;

            _appSetting.LocalTest.Returns(isLocalTest);
            _authService.ReCheckIn(Arg.Any<ReCheckInCriteria>()).Returns(isKickedOut);

            //Action
            var controller = CreateController(currentSession);
            var result = controller.ReCheckIn() as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(isKickedOut, data.IsSuccess);
        }

        private AccountController CreateController(ICurrentSession currentSession = null)
        {
            MockingHelpers.SetUpHttpContextSession(currentSession);
            return new AccountController(_appSetting, _authService, _accountService, _systemSettings, _formsAuthenticationWrapper);
        }
    }
}
