using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Core.Session;
using Fanex.AffiliateAdmin.Core.Utilities;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Models;
using Fanex.AffiliateAdmin.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests
{
    public class HomeControllerTest
    {

        private readonly IAppSetting _appSetting;
        private readonly IHomeService _homeService;
        private readonly ISystemSettingService _systemSettingService;
        public HomeControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _homeService = Substitute.For<IHomeService>();
            _systemSettingService = Substitute.For<ISystemSettingService>();
        }

        [Fact]
        public void Index_WhenCall_ReturnCorrectView()
        {
            //Arrange
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.HasReadPolicy(Arg.Any<string>()).Returns(true);
            currentSession.IsIT.Returns(true);
            currentSession.CurrencyList.Returns(new List<Currency>());

            MockingHelpers.SetUpHttpContextSession(value: currentSession);

            _appSetting.BaseExchange.Returns(new Exchange { Code = "USD" });
            _homeService.GeneralInfoSel(Arg.Any<HomeGeneralInfoCriteria>()).Returns(new HomeViewModel());

            //Action
            var controller = CreateController();
            var result = controller.Index() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model);
        }

        [Fact]
        public void Index_WhenCall_RedirectResult()
        {
            //Arrange
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.HasReadPolicy(Arg.Any<string>()).Returns(false);
            currentSession.IsIT.Returns(false);
            currentSession.CurrencyList.Returns(new List<Currency>());

            MockingHelpers.SetUpHttpContextSession(value: currentSession);

            //Action
            var controller = CreateController();
            var result = controller.Index() as RedirectResult;

            //Assert
            Assert.Equal("~/Home/UserOnlineList", result.Url);
        }

        [Theory]
        [InlineData("en-US", "English")]
        [InlineData("abc", "")]
        public void Language_WhenCall_RedirectResult(string languageId, string languageName)
        {
            //Arrange
            _appSetting.BaseLanguages.Returns(new List<Language> { new Language { LanguageID = languageId, LanguageName = languageName } });
            _homeService.GetSystemDateTime(Arg.Any<SystemDateTimeCriteria>()).Returns(new SystemDateTime());

            MockingHelpers.SetUpHttpContextSession("AffiliateLanguage", "en-US");

            //Action
            var controller = CreateController();
            var result = controller.Language() as PartialViewResult;
            var model = result.Model as Tuple<IEnumerable<Language>, string, SystemDateTime>;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotEmpty(model.Item1);
            Assert.Equal(languageName, model.Item2);
            Assert.NotNull(model.Item3);
        }

        [Fact]
        public void Menu_WhenCall_ReturnPartialView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.Menu() as PartialViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Fact]
        public void UserOnlineList_GET_WhenCall_ReturnViewResult()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.UserOnlineList() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Fact]
        public void UserOnlineList_POST_WhenCall_ReturnCorrectData()
        {
            //Arrange
            var currentSession = Substitute.For<ICurrentSession>();
            MockingHelpers.SetUpHttpContextSession(value: currentSession);
            _homeService.UserOnlineList(Arg.Any<HomeOnlineListCriteria>()).Returns(prs =>
            {
                var criteria = prs[0] as HomeOnlineListCriteria;
                criteria.NRows = 1;

                return new List<UserOnline> { new UserOnline() };
            });

            //Action
            var controller = CreateController();
            var result = controller.UserOnlineList(new HomeOnlineListCriteria()) as JsonResult;
            var data = result.Data;

            //Assert
            Assert.NotNull(data);
        }

        private HomeController CreateController()
            => new HomeController(_appSetting, _homeService, _systemSettingService);
    }
}
