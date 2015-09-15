// --------------------------------------------------------------------------
// <copyright file="CompanyWinLossByProductDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Data;
    using Common.Extensions;
    using Criterias;
    using Data;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class CompanyWinLossByProductDataAccess : CompanyWinLossDataAccess, ICompanyWinLossByProductDataAccess
    {
        /// <summary>
        /// Get data for company win-loss.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Company win-loss report.</returns>
        public CompanyWinlossByProductResult GetCompanyWinLossByProduct(CompanyWinlossByProductCriteria criteria)
        {
            var companyWinlossReport = new CompanyWinlossByProductResult();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    using (var reader = objectDb.ExecuteReader(param))
                    {
                        companyWinlossReport.CompanyWinlossList = GetCompanyWinLossByProductList(reader, criteria.BaseCurrency, criteria.GroupBy);

                        companyWinlossReport.TotalCompanyWinlossInMemberCurrencies = GetCompanyWinLossSumUpByMemberCurrency(reader, criteria.BaseCurrency);

                        companyWinlossReport.TotalCompanyWinlossInSystemCurrency = GetCompanyWinLossSumUpBySystemCurrency(reader);
                    }

                    GetOutputParams(criteria, objectDb);
                }
            }

            return companyWinlossReport;
        }

        protected override void GetColumnExtra(IDataReader reader, CompanyWinLossItem companyWinloss)
        {
            companyWinloss.ProductID = reader.ToInt("ProductID");

            var productName = reader.ToString("ProductName");
            var translatedProduct = Lang.ResourceManager.GetString(productName.Replace(" ", string.Empty));
            companyWinloss.ProductName = string.IsNullOrEmpty(translatedProduct) ? productName : translatedProduct;
        }
    }
}