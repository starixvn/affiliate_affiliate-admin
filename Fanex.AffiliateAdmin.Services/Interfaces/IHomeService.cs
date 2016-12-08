namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Models;

    /// <summary>
    /// Interface Account service.
    /// </summary>
    public interface IHomeService
    {
        /// <summary>
        /// Get general information (win lost, bonus, pending affiliate,....).
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List home view model.</returns>
        HomeViewModel GeneralInfoSel(HomeGeneralInfoCriteria criteria);

        /// <summary>
        /// Get user online list.
        /// </summary>
        /// <param name="criteria">Home online criteria.</param>
        /// <returns>List user online.</returns>
        IEnumerable<UserOnline> UserOnlineList(HomeOnlineListCriteria criteria);

        /// <summary>
        /// Get Affiliate System DateTime.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return System DateTime.</returns>
        SystemDateTime GetSystemDateTime(SystemDateTimeCriteria criteria);
    }
}