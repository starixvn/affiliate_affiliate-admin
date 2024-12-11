namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;
    using System.Collections.Generic;

    /// <summary>
    /// Interface service.
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List email history.</returns>
        IEnumerable<ExchangeRate> GetList(ExchangeSelCriteria criteria);

        /// <summary>
        /// Get default transaction fee.
        /// </summary>
        /// <param name="criteria">Default transaction fee.</param>
        /// <returns>Transaction Fee.</returns>
        PspExchange GetDefaultPspExchange(GetDefaultPspExchangeCriteria criteria);
    }
}