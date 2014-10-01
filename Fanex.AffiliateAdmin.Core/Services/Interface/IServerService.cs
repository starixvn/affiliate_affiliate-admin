using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;

namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    public interface IServerService
    {
        /// <summary>
        /// Get Affiliate System DateTime.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return System DateTime.</returns>
        SystemDateTime GetSystemDateTime(SystemDateTimeCriteria criteria); 
    }
}