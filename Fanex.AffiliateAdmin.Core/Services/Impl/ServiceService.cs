using System.Linq;
using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.Common.Helper;

namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
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