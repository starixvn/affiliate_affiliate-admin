using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Session;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Services;
using NSubstitute;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests.Reports
{
    public class CompanyWinlossByProductControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly IReportService _reportService;
        private readonly ISystemSettings _systemSettings;
        private readonly ICommGroupService _commGroupService;

        public CompanyWinlossByProductControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _reportService = Substitute.For<IReportService>();
            _commGroupService = Substitute.For<ICommGroupService>();
            _systemSettings = MockingHelpers.GetSystemSettings();
        }

        [Fact]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();
            _reportService.GetCompanyWinlossByProduct(Arg.Any<CompanyWinlossByProductCriteria>()).Returns(new CompanyWinlossByProductResult
            {
                CompanyWinlossList = new List<CompanyWinLossItem> { new CompanyWinLossItem() },
                TotalCompanyWinlossInMemberCurrencies = new List<CompanyWinLossItem> { new CompanyWinLossItem() },
                TotalCompanyWinlossInSystemCurrency = new List<CompanyWinLossItem> { new CompanyWinLossItem() }
            });

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.GetData(new CompanyWinlossByProductCriteria { ByAffiliate = true, GroupBy = 1 }) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void GetData_WhenCall_CurrencyList_ReturnCorrectData()
        {
            //Arrange
            ICurrentSession currentSession = Substitute.For<ICurrentSession>();
            currentSession.CurrencyList.Returns(new List<Currency> { new Currency { CurrencyCode = "USD" } });
            MockingHelpers.SetUpHttpContextSession(currentSession);
            _reportService.GetCompanyWinlossByProduct(Arg.Any<CompanyWinlossByProductCriteria>()).Returns(new CompanyWinlossByProductResult
            {
                CompanyWinlossList = new List<CompanyWinLossItem>(),
                TotalCompanyWinlossInMemberCurrencies = new List<CompanyWinLossItem>(),
                TotalCompanyWinlossInSystemCurrency = new List<CompanyWinLossItem>(),
            });

            //Action
            var controller = CreateController();
            var result = controller.GetData(new CompanyWinlossByProductCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        private CompanyWinlossByProductController CreateController()
        {
            return new CompanyWinlossByProductController(_appSetting, _reportService, _systemSettings, _commGroupService);
        }
    }
}
