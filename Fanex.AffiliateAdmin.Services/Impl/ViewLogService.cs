// -----------------------------------------------------------------------
// <copyright file="ViewLogService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Impl
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services.Interface;
    using Fanex.Common.Helper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewLogService : QueryService, IViewLogService
    {
        /// <summary>
        /// Get Log type.
        /// </summary>
        /// <param name="criteria">Log type criteria.</param>
        /// <returns>List Log Type.</returns>
        public IEnumerable<LogType> GetLogTypes(LogTypeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<LogType>(criteria);
        }

        /// <summary>
        /// Get List Log.
        /// </summary>
        /// <param name="criteria">View Log list criteria.</param>
        /// <returns>List View Log.</returns>
        public IEnumerable<ViewLog> GetViewLogList(ViewLogListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetDataOutput<ViewLog>(criteria);
        }
    }
}
