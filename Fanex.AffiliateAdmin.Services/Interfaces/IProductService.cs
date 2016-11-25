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