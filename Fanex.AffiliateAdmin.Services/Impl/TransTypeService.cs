// -----------------------------------------------------------------------
// <copyright file="TransTypeService.cs" company="Microsoft">
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
    public class TransTypeService : QueryService, ITransTypeService
    {
        /// <summary>
        /// Get List Transaction Type.
        /// </summary>
        /// <param name="criteria">Transaction Type Criteria.</param>
        /// <returns>List Transaction Type.</returns>
        public IEnumerable<TransType> GetTransTypes(TransTypeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<TransType>(criteria);
        }
    }
}
