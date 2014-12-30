// --------------------------------------------------------------------------
// <copyright file="IProductService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.13">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Models;

    /// <summary>
    /// Interface Product service.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get product list of affiliate.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Product list.</returns>
        IEnumerable<SimpleProduct> GetProductList(GetProductByAffIdCriteria criteria);
    }
}