namespace Fanex.AffiliateAdmin.Services
{
    using Core.Services;
    using Criterias;
    using DTO;
    using System.Collections.Generic;

    /// <summary>
    /// Email History Service.
    /// </summary>
    public class ExchangeRateService : QueryService, IExchangeRateService
    {
        #region Fields

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateService" /> class.
        /// </summary>
        public ExchangeRateService()
            : base(null)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List data.</returns>
        public IEnumerable<ExchangeRate> GetList(ExchangeSelCriteria criteria)
        {
            return GetDataOutput<ExchangeRate>(criteria);
        }

        public PspExchange GetDefaultPspExchange(GetDefaultPspExchangeCriteria criteria)
        {
            return GetEntity<PspExchange>(criteria);
        }
    }
}