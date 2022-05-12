using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Models.Reports;
using Fanex.AffiliateAdmin.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests.Reports
{
    public class ReportControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly IReportService _reportService;
        private readonly ISystemSettings _systemSettings;
        private readonly ICommGroupService _commGroupService;

        public ReportControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _reportService = Substitute.For<IReportService>();
            _commGroupService = Substitute.For<ICommGroupService>();
            _systemSettings = MockingHelpers.GetSystemSettings();
        }

        #region Company Win Loss Report.
        [Fact]
        public void Index_WhenCall_RedirectToCorrectAction()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.Index() as RedirectToRouteResult;

            //Assert
            Assert.Equal("CompanyWinloss", result.RouteValues["action"]);
        }

        [Fact]
        public void CompanyWinloss_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.CompanyWinloss() as ViewResult;

            //Assert
            Assert.NotNull(result.ViewBag.Products);
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model as CompanyWinlossViewModel);
        }

        [Fact]
        public void CompanyWinlossByAffiliate_WhenCall_ReturnCorrectView()
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);

            var result = controller.CompanyWinlossByAffiliate(new CompanyWLByAffiliateViewModel()) as ViewResult;

            //Assert
            Assert.NotNull(result.ViewBag.Products);
            Assert.Equal("CompanyWinlossByAffiliate", result.ViewName);
            Assert.NotNull(result.Model as CompanyWLByAffiliateViewModel);
        }

        #endregion

        #region Member Win Loss Report
        [Fact]
        public void MemberWinloss_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.MemberWinloss(new MemberWinlossViewModel()) as ViewResult;

            //Assert
            Assert.NotNull(result.ViewBag.Products);
            Assert.Equal("MemberWinloss", result.ViewName);
            Assert.NotNull(result.Model as MemberWinlossViewModel);
        }

        [Fact]
        public void MemberWinlossByAffiliate_WhenCall_ReturnCorrectView()
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            _systemSettings.BaseExchange.Returns(new Exchange { Code = "IDR" });

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);

            var result = controller.MemberWinlossByAffiliate(new MemberWinlossByAffiliateViewModel()) as ViewResult;

            //Assert
            Assert.NotNull(result.ViewBag.Products);
            Assert.Equal("MemberWinlossByAffiliate", result.ViewName);
            Assert.NotNull(result.Model as MemberWinlossByAffiliateViewModel);
        }
        #endregion
        #region Member Statistics
        [Fact]
        public void MemberStatistics_GET_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.MemberStatistics() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(result.Model as MemberStatisticsViewModel);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public void MemberStatistics_POST_WhenCall_ReturnCorrectJsonResult(byte groupBy)
        {
            //Arrange
            _systemSettings.SystemStartDate.Returns(new DateTime(2022, 05, 05));
            _systemSettings.SystemDateTime.Returns(new DateTime(2022, 06, 03));
            _reportService.GetMemberStatistics(Arg.Any<MemberStatisticReportCriteria>()).Returns(new List<MemberStatistic> { new MemberStatistic() });

            //Action
            var controller = CreateController();
            var result = controller.MemberStatistics(new MemberStatisticReportCriteria { GroupBy = groupBy }) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }
        #endregion

        #region Affiliate Win Loss Report
        [Theory]
        [InlineData("")]
        [InlineData("1,2")]
        public void AffiliateWinLoss_WhenCall_ReturnCorrectView(string currencies)
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.AffiliateWinLoss(new AffiliateWinLossViewModel { Currencies = currencies }) as ViewResult;

            //Assert
            Assert.NotNull(result.ViewBag.Products);
            Assert.NotNull(result.Model as AffiliateWinLossViewModel);
        }

        [Fact]
        public void AffiliateWinLossByAffiliate_WhenCall_ReturnCorrectView()
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);

            var result = controller.AffiliateWinLossByAffiliate(new AffiliateWLByAffiliateViewModel()) as ViewResult;

            //Assert
            Assert.NotNull(result.ViewBag.Products);
            Assert.NotNull(result.Model as AffiliateWLByAffiliateViewModel);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public void QualifiedMembers_WhenCall_ReturnCorrectView(int groupId)
        {
            //Arrange
            _reportService.GetQualifiedMembers(Arg.Any<QualifiedMembersReportCriteria>()).Returns(new List<QualifiedMember> { new QualifiedMember() });

            //Action
            var controller = CreateController();
            var result = controller.QualifiedMembers(new QualifiedMembersReportCriteria { GroupId = groupId }) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.Single(result.Model as IEnumerable<QualifiedMember>);
        }
        #endregion

        #region Affiliate Commission Report
        [Fact]
        public void AffiliateCommission_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.AffiliateCommission() as ViewResult;

            //Assert
            Assert.NotNull(result.ViewBag.Products);
            Assert.NotNull(result.Model as AffiliateCommissionViewModel);
        }

        [Fact]
        public void AffiliateCommissionByAffiliate_WhenCall_ReturnCorrectView()
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();

            //Action
            var controller = CreateController();
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);

            var result = controller.AffiliateCommissionByAffiliate(new AffiliateCommissionByAffiliateViewModel()) as ViewResult;

            //Assert
            Assert.NotNull(result.ViewBag.Products);
            Assert.NotNull(result.Model as AffiliateCommissionByAffiliateViewModel);
        }
        #endregion

        private ReportController CreateController()
        {
            MockingHelpers.SetUpEmptyHttpContextSession();

            return new ReportController(_appSetting, _reportService, _systemSettings, _commGroupService);
        }
    }
}
