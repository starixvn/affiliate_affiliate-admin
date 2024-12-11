namespace Fanex.AffiliateAdmin.Services
{
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Account Service.
    /// </summary>
    public class HomeService : QueryService, IHomeService
    {
        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List home view model.</returns>
        public HomeViewModel GeneralInfoSel(HomeGeneralInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var model = GetEntity<HomeViewModel>(criteria);
            return model ?? new HomeViewModel();
        }

        /// <summary>
        /// User online list.
        /// </summary>
        /// <param name="criteria">Home online criteria.</param>
        /// <returns>List user online.</returns>
        public IEnumerable<UserOnline> UserOnlineList(HomeOnlineListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<UserOnline>(criteria);
        }

        /// <summary>
        /// Get System Date time.
        /// </summary>
        /// <param name="criteria">System date time criteria.</param>
        /// <returns>System date time.</returns>
        public SystemDateTime GetSystemDateTime(SystemDateTimeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<SystemDateTime>(criteria);
            return list.FirstOrDefault();
        }
    }
}