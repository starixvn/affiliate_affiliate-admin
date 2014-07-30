// -----------------------------------------------------------------------
// <copyright file="CountryService.cs" company="Microsoft">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CountryService : QueryService, ICountryService
    {
        /// <summary>
        /// Get all country.
        /// </summary>
        /// <param name="criteria">Country criteria.</param>
        /// <returns>List Country.</returns>
        public IEnumerable<Country> GetAll(CountryCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<Country>(criteria);
        }
    }
}
