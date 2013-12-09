// -----------------------------------------------------------------------
// <copyright file="ITransTypeService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Interface
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITransTypeService
    {
        /// <summary>
        /// Get List Transaction Type.
        /// </summary>
        /// <param name="criteria">Transaction Type Criteria.</param>
        /// <returns>List Transaction Type.</returns>
        IEnumerable<TransType> GetTransTypes(TransTypeCriteria criteria);
    }
}
