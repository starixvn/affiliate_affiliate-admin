// -----------------------------------------------------------------------
// <copyright file="StatementService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Impl
{
    using System.Collections.Generic;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StatementService : QueryService, IStatementService
    {
        /// <summary>
        /// Get List Affiliate statement.
        /// </summary>
        /// <param name="criteria">Statement Criteria.</param>
        /// <returns>List affiliate statement.</returns>
        public IEnumerable<Statement> GetAffiliateStatement(StatementCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<Statement>(criteria);
        }
    }
}
