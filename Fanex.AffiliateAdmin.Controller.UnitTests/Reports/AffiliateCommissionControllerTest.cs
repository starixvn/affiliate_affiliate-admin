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
    public class AffiliateCommissionControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly IReportService _reportService;
        private readonly ISystemSettings _systemSettings;
        private readonly ICommGroupService _commGroupService;

        public AffiliateCommissionControllerTest()
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
            _reportService.GetAffiliateCommission(Arg.Any<AffiliateCommissionCriteria>()).Returns(new AffiliateCommissionResult {
                AffiliateCommissionList = new List<AffiliateCommission> { new AffiliateCommission() },
                AffiliateCommissionInAffiliateCurrency = new List<AffiliateCommission> { new AffiliateCommission() },
                AffiliateCommissionInSystemCurrency = new List<AffiliateCommission> { new AffiliateCommission() }
            });

            //Action
            var controller = CreateController();
            var result = controller.GetData(new AffiliateCommissionCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void GetData_WhenCall_CurrencyList_ReturnCorrectData()
        {
            //Arrange
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.CurrencyList.Returns(new List<Currency> { new Currency { CurrencyCode = "USD" } });
            MockingHelpers.SetUpHttpContextSession(currentSession);
            _reportService.GetAffiliateCommission(Arg.Any<AffiliateCommissionCriteria>()).Returns(new AffiliateCommissionResult
            {
                AffiliateCommissionList = new List<AffiliateCommission>(),
                AffiliateCommissionInAffiliateCurrency = new List<AffiliateCommission>(),
                AffiliateCommissionInSystemCurrency = new List<AffiliateCommission>()
            });

            //Action
            var controller = CreateController();
            var result = controller.GetData(new AffiliateCommissionCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void GetDataByAffiliate_WhenCall_ReturnCorrectData()
        {
            //Arrange
            MockingHelpers.SetUpEmptyHttpContextSession();
            _reportService.GetAffiliateCommission(Arg.Any<AffiliateCommissionCriteria>()).Returns(new AffiliateCommissionResult
            {
                AffiliateCommissionList = new List<AffiliateCommission> { new AffiliateCommission() },
                AffiliateCommissionInAffiliateCurrency = new List<AffiliateCommission> { new AffiliateCommission() },
                AffiliateCommissionInSystemCurrency = new List<AffiliateCommission> { new AffiliateCommission() }
            });

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(MockingHelpers.GetHttpRequest());
            var result = controller.GetDataByAffiliate(new AffiliateCommissionCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        private AffiliateCommissionController CreateController() =>
            new AffiliateCommissionController(_appSetting, _reportService, _systemSettings, _commGroupService);
    }
}
