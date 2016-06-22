namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

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