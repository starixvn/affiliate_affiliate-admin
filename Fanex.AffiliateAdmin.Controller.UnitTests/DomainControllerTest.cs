using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.AffiliateAdmin.Controller.UnitTests
{
    public class DomainControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly IDomainService _domainService;
        private readonly ISystemSettings _systemSettings;
        private readonly ISystemSettingService _systemSettingService;

        public DomainControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _domainService = Substitute.For<IDomainService>();
            _systemSettingService = Substitute.For<ISystemSettingService>();
            _systemSettings = MockingHelpers.GetSystemSettings();
        }

        private DomainController CreateController() =>
            new DomainController(_appSetting, _domainService, _systemSettings, _systemSettingService);
    }
}
