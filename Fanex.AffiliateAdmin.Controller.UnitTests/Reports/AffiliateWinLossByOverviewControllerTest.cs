using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Session;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests.Reports
{
    public class AffiliateWinLossByOverviewControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly IReportService _reportService;
        private readonly ISystemSettings _systemSettings;
        private readonly ICommGroupService _commGroupService;

        public AffiliateWinLossByOverviewControllerTest()
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
            MockingHelpers.SetUpEmptyHttpContextSession();
            _reportService.GetAffiliateWinLossOverview(Arg.Any<AffiliateWinLossOverviewCriteria>()).Returns(new AffiliateWinLossByOverviewResult
            {
                CommissionList = new List<AffiliateWinLossItem> { new AffiliateWinLossItem() },
                CommissionInAffiliateCurrency = new List<AffiliateWinLossItem> { new AffiliateWinLossItem() },
                CommissionInSystemCurrency = new AffiliateWinLossItem()
            });

            //Action
            var controller = CreateController();
            var result = controller.GetData(new AffiliateWinLossOverviewCriteria()) as JsonResult;

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
            _reportService.GetAffiliateWinLossOverview(Arg.Any<AffiliateWinLossOverviewCriteria>()).Returns(new AffiliateWinLossByOverviewResult
            {
                CommissionList = new List<AffiliateWinLossItem>(),
                CommissionInAffiliateCurrency = new List<AffiliateWinLossItem>()
            });

            //Action
            var controller = CreateController();
            var result = controller.GetData(new AffiliateWinLossOverviewCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void GetDataByAffiliate_WhenCall_ReturnCorrectData()
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();
            _reportService.GetAffiliateWinLossOverview(Arg.Any<AffiliateWinLossOverviewCriteria>()).Returns(new AffiliateWinLossByOverviewResult
            {
                CommissionList = new List<AffiliateWinLossItem> { new AffiliateWinLossItem() },
                CommissionInAffiliateCurrency = new List<AffiliateWinLossItem> { new AffiliateWinLossItem() },
                CommissionInSystemCurrency = new AffiliateWinLossItem()
            });

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(MockingHelpers.GetHttpRequest());
            var result = controller.GetDataByAffiliate(new AffiliateWinLossOverviewCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        private AffiliateWinLossByOverviewController CreateController()
        {
            return new AffiliateWinLossByOverviewController(_appSetting, _reportService, _systemSettings, _commGroupService);
        }
    }
}
