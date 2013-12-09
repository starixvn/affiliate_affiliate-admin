// --------------------------------------------------------------------------
// <copyright file="IExchangeRateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.29">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Interface
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
        IEnumerable<ExchangeRate> GetList(ExchangeRateCriteria criteria);

        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void ExecuteOutput(ExchangeRateCriteria criteria);
    }
}