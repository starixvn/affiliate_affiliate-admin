// -----------------------------------------------------------------------
// <copyright file="IViewLogService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IViewLogService
    {
        /// <summary>
        /// Get Log type.
        /// </summary>
        /// <param name="criteria">Log type criteria.</param>
        /// <returns>List Log Type.</returns>
        IEnumerable<LogType> GetLogTypes(LogTypeCriteria criteria);

        /// <summary>
        /// Get List Log.
        /// </summary>
        /// <param name="criteria">View Log list criteria.</param>
        /// <returns>List View Log.</returns>
        IEnumerable<ViewLog> GetViewLogList(ViewLogListCriteria criteria);
    }
}