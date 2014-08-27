// --------------------------------------------------------------------------
// <copyright file="ExchangeRateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.29">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;

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
            ////: this(null)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List data.</returns>
        public IEnumerable<ExchangeRate> GetList(ExchangeRateCriteria criteria)
        {
            return GetDataOutput<ExchangeRate>(criteria);
        }

        public PspExchange GetDefaultPspExchange(GetDefaultPspExchangeCriteria criteria)
        {
            return GetEntity<PspExchange>(criteria);
        }
    }
}