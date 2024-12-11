namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;
    using System.Collections.Generic;

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