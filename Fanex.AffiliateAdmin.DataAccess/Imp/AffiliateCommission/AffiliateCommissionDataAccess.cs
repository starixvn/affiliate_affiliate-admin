namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Common.Extensions;

    /// <summary>
    /// Affiliate Commission Data Access.
    /// </summary>
    public class AffiliateCommissionDataAccess : ReportDataAccess
    {
        protected virtual void GetColumnExtra(IDataReader reader, AffiliateCommission affiliateCommission)
        {
        }

        protected virtual void GetColumnExtra(IDataReader reader, AffiliateCommission affiliateCommission, string colSuffix)
        {
        }

        #region Helpers for commission

        /// <summary>
        /// Get simple commission list.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Commission list.</returns>
        protected IEnumerable<AffiliateCommission> GetAffiliateCommissionList(IDataReader reader)
        {
            var result = new List<AffiliateCommission>();

            while (reader.Read())
            {
                var commission = new AffiliateCommission
                {
                    RowIdx = reader.ToLong("RowIDX"),
                    AffId = reader.ToInt("AffId"),
                    Username = reader.ToString("Username"),
                    WinlostDate = reader.ToDateTime("WinlostDate"),
                    GroupID = reader.ToInt("GroupID"),
                    GroupName = reader.ToString("GroupName"),
                    Currency = reader.ToString("Currency"),
                    ProductId = reader.ToInt("ProductId"),
                    ProductName = reader.ToString("ProductName"),
                    TotalQM = reader.ToInt("TotalQM"),
                    CommissionPercent = reader.ToDecimal("CommissionPercent")
                };

                var productName = commission.ProductName;
                var translatedProduct = Lang.ResourceManager.GetString(productName.Replace(" ", string.Empty));
                commission.ProductName = string.IsNullOrEmpty(translatedProduct) ? productName : translatedProduct;

                GetColumnExtra(reader, commission);

                GetCommissionColumns(reader, commission, string.Empty);

                GetCommissionColumns(reader, commission, SysColSuffix);

                result.Add(commission);
            }

            return result;
        }

        /// <summary>
        /// Get total commission in affiliate currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Total commission list.</returns>
        protected IEnumerable<AffiliateCommission> GetTotalAffiliateCommissionInAffiliateCurrency(IDataReader reader)
        {
            var result = new List<AffiliateCommission>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var commission = new AffiliateCommission
                        {
                            Currency = reader.ToString("Currency"),
                            ProductId = reader.ToInt("ProductId"),
                            ProductName = reader.ToString("ProductName"),
                            TotalQM = reader.ToInt("TotalQM"),
                            CommissionPercent = reader.ToDecimal("CommissionPercent")
                        };

                    var productName = commission.ProductName;
                    var translatedProduct = Lang.ResourceManager.GetString(productName.Replace(" ", string.Empty));
                    commission.ProductName = string.IsNullOrEmpty(translatedProduct) ? productName : translatedProduct;

                    GetColumnExtra(reader, commission);

                    GetCommissionColumns(reader, commission, string.Empty);

                    GetCommissionColumns(reader, commission, SysColSuffix);

                    result.Add(commission);
                }
            }

            return result;
        }

        /// <summary>
        /// Get total commission in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Total commission.</returns>
        protected IEnumerable<AffiliateCommission> GetTotalAffiliateCommissionInSystemCurrency(IDataReader reader)
        {
            var result = new List<AffiliateCommission>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var commission = new AffiliateCommission
                        {
                            ProductId = reader.ToInt("ProductId"),
                            ProductName = reader.ToString("ProductName"),
                            TotalQM = reader.ToInt("TotalQM"),
                            CommissionPercent = reader.ToDecimal("CommissionPercent")
                        };

                    var productName = commission.ProductName;
                    var translatedProduct = Lang.ResourceManager.GetString(productName.Replace(" ", string.Empty));
                    commission.ProductName = string.IsNullOrEmpty(translatedProduct) ? productName : translatedProduct;

                    GetColumnExtra(reader, commission);

                    GetCommissionColumns(reader, commission, SysColSuffix);

                    result.Add(commission);
                }
            }

            return result;
        }

        /// <summary>
        /// Get commission columns in affiliate currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="commission">Commission object.</param>
        /// <param name="colSuffix">Column suffix.</param>
        private void GetCommissionColumns(IDataReader reader, AffiliateCommission commission, string colSuffix)
        {
            SetPropertyValue(reader, commission, string.Format("NetWinlost{0}", colSuffix));
            SetPropertyValue(reader, commission, string.Format("Commission{0}", colSuffix));
            SetPropertyValue(reader, commission, string.Format("TotalPayment{0}", colSuffix));

            GetColumnExtra(reader, commission, colSuffix);
        }

        #endregion Helpers for commission
    }
}