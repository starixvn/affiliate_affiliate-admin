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
    public class MemberWinlossByProductControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly IReportService _reportService;
        private readonly ISystemSettings _systemSettings;
        private readonly ICommGroupService _commGroupService;

        public MemberWinlossByProductControllerTest()
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
            _reportService.GetMemberWinlossByProduct(Arg.Any<MemberWinlossByProductCriteria>()).Returns(new MemberWinLossByProductResult
            {
                MemberWinlossList = new List<MemberWinLossItem> { new MemberWinLossItem() },
                TotalMemberWinlossInMemberCurrencies = new List<MemberWinLossItem> { new MemberWinLossItem() },
                TotalMemberWinlossInSystemCurrency = new List<MemberWinLossItem> { new MemberWinLossItem() }
            });

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.GetData(new MemberWinlossByProductCriteria { ByAffiliate = true, GroupBy = 1 }) as JsonResult;

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
            _reportService.GetMemberWinlossByProduct(Arg.Any<MemberWinlossByProductCriteria>()).Returns(new MemberWinLossByProductResult
            {
                MemberWinlossList = new List<MemberWinLossItem>(),
                TotalMemberWinlossInMemberCurrencies = new List<MemberWinLossItem>(),
                TotalMemberWinlossInSystemCurrency = new List<MemberWinLossItem>(),
            });

            //Action
            var controller = CreateController();
            var result = controller.GetData(new MemberWinlossByProductCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        private MemberWinlossByProductController CreateController()
        {
            return new MemberWinlossByProductController(_appSetting, _reportService, _systemSettings, _commGroupService);
        }
    }
}
