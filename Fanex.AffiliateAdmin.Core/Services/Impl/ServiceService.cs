namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Helper;
    using System.Linq;

    /// <summary>
    /// Service Service.
    /// </summary>
    public class ServiceService : QueryService, IServerService
    {
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