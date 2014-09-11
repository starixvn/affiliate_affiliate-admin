// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyWinlossByCurrency
    {
        public string Currency { get; set; }

        public IEnumerable<CompanyWinloss> CompanyWinlossByProductList;

        public CompanyWinloss TotalCompanyWinlossByProduct;
    }
}
