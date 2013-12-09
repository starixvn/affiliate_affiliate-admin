// -----------------------------------------------------------------------
// <copyright file="ICountryService.cs" company="Microsoft">
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
    public interface ICountryService
    {
        /// <summary>
        /// Get all country.
        /// </summary>
        /// <param name="criteria">Country criteria.</param>
        /// <returns>List Country.</returns>
        IEnumerable<Country> GetAll(CountryCriteria criteria);
    }
}
