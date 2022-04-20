namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using ONEbook.UM.Models;

    public interface IUnderMaintenanceService
    {
        UnderMaintenanceTime GetUnderMaintenanceTime();

        bool IsUnderMaintenance();
    }
}
