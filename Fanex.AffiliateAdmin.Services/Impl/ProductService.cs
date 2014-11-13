// --------------------------------------------------------------------------
// <copyright file="ProductService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.13">Created</change>
// </history>


namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Product Service.
    /// </summary>
    public class ProductService : QueryService, IProductService
    {
        /// <summary>
        /// Get product list of affiliate.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Product list.</returns>
        public IEnumerable<SimpleProduct> GetProductList(GetProductByAffIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<SimpleProduct>(criteria);
        }
    }
}