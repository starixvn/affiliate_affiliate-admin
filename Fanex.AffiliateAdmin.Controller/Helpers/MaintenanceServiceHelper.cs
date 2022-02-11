using Fanex.Caching;
using ONEbook.UM;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.NETFramework;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;

namespace Fanex.AffiliateAdmin.Controller.Helpers
{
    public static class MaintenanceServiceHelper
    {
        public static MaintenanceService GetMaintenanceService()
        {
            var umConfig = new UMClientConfiguration().Initialize();

            UMClientManager
               .UseConfig(umConfig)
               .UseCacheService(new UMCacheService(new CacheService()))
               .UseRestClient(new UMRestClient(umConfig))
               .UseHttpContext(new HttpContextHelper());

            return new MaintenanceService(
                                  UMClientManager.RestClient,
                                  UMClientManager.CacheService,
                                  UMClientManager.UMClientConfigs,
                                  UMClientManager.HttpContext);
        }
    }
}
