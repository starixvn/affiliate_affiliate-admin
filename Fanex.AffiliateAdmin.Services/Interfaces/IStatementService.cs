// -----------------------------------------------------------------------
// <copyright file="IStatementService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IStatementService
    {
        /// <summary>
        /// Get List Affiliate statement.
        /// </summary>
        /// <param name="criteria">Statement Criteria.</param>
        /// <returns>List affiliate statement.</returns>
        IEnumerable<Statement> GetAffiliateStatement(StatementCriteria criteria);
    }
}
