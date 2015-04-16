// -----------------------------------------------------------------------
// <copyright file="StatementService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;

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

            var statements = GetData<Statement>(criteria);

            foreach (var statement in statements)
            {
                statement.OriginalStatus = statement.Status;

                if (!string.IsNullOrEmpty(statement.Status))
                {
                    var status = Lang.ResourceManager.GetString(statement.Status.Replace("/", string.Empty));
                    statement.Status = string.IsNullOrEmpty(status) ? statement.Status : status;
                }

                if (!string.IsNullOrEmpty(statement.Creator))
                {
                    var creator = Lang.ResourceManager.GetString(statement.Creator);
                    statement.Creator = string.IsNullOrEmpty(creator) ? statement.Creator : creator;
                }

                if (!string.IsNullOrEmpty(statement.Description))
                {
                    var description = Lang.ResourceManager.GetString(statement.Description.Replace(" ", string.Empty));
                    statement.Description = string.IsNullOrEmpty(description) ? statement.Description : description;
                }
            }

            return statements;
        }
    }
}