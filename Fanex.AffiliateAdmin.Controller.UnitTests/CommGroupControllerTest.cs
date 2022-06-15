using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Session;
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
    public class CommGroupControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly ISystemSettings _systemSettings;
        private readonly ICommGroupService _commGroupService;
        private readonly IExchangeRateService _exchangeRateService;

        public CommGroupControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _commGroupService = Substitute.For<ICommGroupService>();
            _exchangeRateService = Substitute.For<IExchangeRateService>();
            _systemSettings = MockingHelpers.GetSystemSettings();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(0.5, 0.5)]
        public void Add_GET_WhenCall_ReturnCorrectView(double? depositPercent, double? withdrawalPercent)
        {
            //Arrange
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.CurrencyList.Returns(new List<Currency> { new Currency { CurrencyCode = "USD", CurrencyID = 123 } });
            MockingHelpers.SetUpHttpContextSession(currentSession);

            _exchangeRateService.GetDefaultPspExchange(Arg.Any<GetDefaultPspExchangeCriteria>()).Returns(new PspExchange { DepositPercent = depositPercent, WithdrawalPercent = withdrawalPercent });

            //Action
            var controller = CreateController();
            var result = controller.Add() as ViewResult;

            //Assert
            Assert.NotNull(result.Model as CommGroupInfo);
        }

        [Theory]
        [InlineData(0, "", true, "")]
        [InlineData(0, "messageSuccess", true, "")]
        [InlineData(1, "a message", true, "")]
        [InlineData(29, "", false, "7;1.00;|8;1.00;")]
        [InlineData(29, "a message", false, "7;1.00;|8;1.00;")]
        [InlineData(9999, "", false, "7;1.00;|8;1.00;")]
        public void Add_POST_WhenCall_ReturnCorrectView(int errCode, string errMsg, bool isCopyForAllProducts, string deductionSettingList)
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();

            _commGroupService.When(c => c.ExecuteOutput(Arg.Any<CommGroupInsertCriteria>())).Do(c => {
                var criteria = (c.Args()[0] as CommGroupInsertCriteria);
                criteria.ErrCode = errCode;
                criteria.ErrMsg = errMsg;
            });

            //Action
            var controller = CreateController();
            var result = controller.Add(new CommGroupInsertCriteria {
                GroupId = 123,
                CopyForAllProducts = isCopyForAllProducts,
                DeductionSettingList = deductionSettingList
            }) as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(errCode, data.ErrorCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(29)]
        public void Delete_WhenCall_ReturnCorrectView(int errCode)
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();
            var httpRequest = MockingHelpers.GetHttpRequest();
            _commGroupService.When(c => c.ExecuteOutput(Arg.Any<CommGroupDeleteCriteria>())).Do(c => {
                var criteria = (c.Args()[0] as CommGroupDeleteCriteria);
                criteria.ErrCode = errCode;
            });

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.Delete(123);

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Edit_GET_WhenCallWithNullGroup_ReturnContentResult()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.Edit(123) as ContentResult;

            //Assert
            Assert.Equal("Group is null", result.Content);
        }

        [Fact]
        public void Edit_GET_WhenCallWithGroup_ReturnCorrectView()
        {
            //Arrange
            _commGroupService.GetListData<CommGroupInfo>(Arg.Any<CommGroupInfoCriteria>()).Returns(new List<CommGroupInfo> { new CommGroupInfo() });
            _commGroupService.GetListData<GroupSetting>(Arg.Any<GroupSettingCriteria>()).Returns(new List<GroupSetting> { new GroupSetting() });
            _commGroupService.GetListData<DeductionSetting>(Arg.Any<DeductionSettingCriteria>()).Returns(new List<DeductionSetting> { new DeductionSetting() });

            //Action
            var controller = CreateController();
            var result = controller.Edit(123) as ViewResult;

            //Assert
            Assert.Equal("Edit", result.ViewName);
        }

        [Theory]
        [InlineData(0, "", true, "")]
        [InlineData(0, "messageSuccess", true, "")]
        [InlineData(1, "a message", true, "")]
        [InlineData(29, "", false, "7;1.00;|8;1.00;")]
        [InlineData(29, "a message", false, "7;1.00;|8;1.00;")]
        [InlineData(9999, "", false, "7;1.00;|8;1.00;")]
        public void Edit_POST_WhenCall_ReturnCorrectView(int errCode, string errMsg, bool isCopyForAllProducts, string deductionSettingList)
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();

            _commGroupService.When(c => c.ExecuteOutput(Arg.Any<CommGroupUpdateCriteria>())).Do(c => {
                var criteria = (c.Args()[0] as CommGroupUpdateCriteria);
                criteria.ErrCode = errCode;
                criteria.ErrMsg = errMsg;
            });

            //Action
            var controller = CreateController();
            var result = controller.Edit(new CommGroupUpdateCriteria
            {
                GroupId = 123,
                CopyForAllProducts = isCopyForAllProducts,
                DeductionSettingList = deductionSettingList
            }) as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(errCode, data.ErrorCode);
        }

        [Theory]
        [InlineData(123, 1)]
        [InlineData(123, 2)]
        [InlineData(1, 3)]
        public void GetCurrencyNotification_WhenCall_ReturnCorrectData(int currencyId, int multipliedBy)
        {
            //Arrange
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.CurrencyList.Returns(new List<Currency> { new Currency { CurrencyCode = "USD", CurrencyID = 123, MultipliedBy = multipliedBy } });
            MockingHelpers.SetUpHttpContextSession(currentSession);

            //Action
            var controller = CreateController();
            var result = controller.GetCurrencyNotification(currencyId);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetDefaultPspExchange_WhenCall_ReturnCorrectData()
        {
            //Arrange
            _exchangeRateService.GetDefaultPspExchange(Arg.Any<GetDefaultPspExchangeCriteria>()).Returns(new PspExchange());

            //Action
            var controller = CreateController();
            var result = controller.GetDefaultPspExchange(new GetDefaultPspExchangeCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Theory]
        [InlineData("")]
        [InlineData(",1,2,3")]
        public void GetList_WhenCall_ReturnCorrectData(string groupIdList)
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();
            _commGroupService.GetListData<CommGroup>(Arg.Any<CommGroupCriteria>()).Returns(new List<CommGroup> { new CommGroup() });

            //Action
            var controller = CreateController();
            var result = controller.GetList(new CommGroupCriteria { GroupIDList = groupIdList}) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            var result = controller.Index("") as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ViewSetting_WhenCall_ReturnCorrectView(bool hasDate)
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();
            _commGroupService.GetById(Arg.Any<CommGroupSelByIdCriteria>()).Returns(new List<CommGroup> { new CommGroup() });

            //Action
            var controller = CreateController();
            var result = controller.ViewSetting(new CommGroupSelByIdCriteria { Date = hasDate ? new DateTime(2022, 06, 17) : default(DateTime?) }) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model);
        }

        private CommGroupController CreateController() =>
            new CommGroupController(_appSetting, _systemSettings, _commGroupService, _exchangeRateService);
    }
}
