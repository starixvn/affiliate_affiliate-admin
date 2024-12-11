namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;
    using System.Collections.Generic;

    /// <summary>
    /// Interface Email History service.
    /// </summary>
    public interface IEmailHistoryService
    {
        /// <summary>
        /// Get email history list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List email history.</returns>
        IEnumerable<EmailHistory> GetList(EmailHistorySelCriteria criteria);

        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void ExecuteSP(EmailHistoryDelCriteria criteria);
    }
}