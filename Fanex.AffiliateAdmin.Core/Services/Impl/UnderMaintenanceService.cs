namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    using ONEbook.UM.Services;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using ONEbook.UM.Models;
    using ONEbook.UM;

    public class UnderMaintenanceService : IUnderMaintenanceService
    {
        private readonly IMaintenanceService _maintenaceService;
        public UnderMaintenanceService()
        {
            _maintenaceService = new MaintenanceService(
                                      UMClientManager.RestClient,
                                      UMClientManager.CacheService,
                                      UMClientManager.UMClientConfigs,
                                      UMClientManager.HttpContext);
        }

        public UnderMaintenanceTime GetUnderMaintenanceTime()
        {
            return _maintenaceService.GetUnderMaintenanceTime();
        }

        public bool IsUnderMaintenance()
        {
            return _maintenaceService.IsUnderMaintenance();
        }
    }
}
