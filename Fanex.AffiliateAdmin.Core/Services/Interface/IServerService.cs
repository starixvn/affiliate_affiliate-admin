namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

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