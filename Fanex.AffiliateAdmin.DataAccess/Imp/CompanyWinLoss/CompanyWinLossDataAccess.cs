namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Extensions;

    /// <summary>
    /// Company Win Loss Data Access.
    /// </summary>
    public abstract class CompanyWinLossDataAccess : ReportDataAccess
    {
        protected virtual void GetColumnExtra(IDataReader reader, CompanyWinLossItem companyWinloss)
        {
        }

        protected virtual void GetColumnExtra(IDataReader reader, CompanyWinLossItem companyWinloss, string colSuffix)
        {
        }

        /// <summary>
        /// Get simple company win-loss.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="baseCurrency">Base Currency.</param>
        /// <param name="groupBy">Group By Input.</param>
        /// <returns>Company win-loss list.</returns>
        protected IEnumerable<CompanyWinLossItem> GetCompanyWinLossByProductList(IDataReader reader, byte baseCurrency, int groupBy)
        {
            var result = new List<CompanyWinLossItem>();
            const int GroupByDate = 0;

            while (reader.Read())
            {
                var companyWinloss = new CompanyWinLossItem
                {
                    RowIdx = reader.ToLong("RowIDX"),
                    WinlostDate = reader.ToDateTime("WinlostDate"),
                    Currency = reader.ToString("Currency"),
                    CurrencyId = reader.ToShort("CurrencyId"),
                    BetCount = reader.ToInt("BetCount"),
                    ActivePlayer = reader.ToInt("ActivePlayer"),
                };

                GetColumnExtra(reader, companyWinloss);

                GetCompanyWinLossColumnsBasedOnBaseCurrency(baseCurrency, reader, companyWinloss);

                if (groupBy == GroupByDate)
                {
                    companyWinloss.ExRate = reader.ToDouble("ExRate");
                    companyWinloss.AffExRate = reader.ToDouble("AffExRate");
                }

                result.Add(companyWinloss);
            }

            return result;
        }

        /// <summary>
        /// Get total company win-loss in member currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="baseCurrency">Base Currency.</param>
        /// <returns>Total company win-loss list.</returns>
        protected IEnumerable<CompanyWinLossItem> GetCompanyWinLossSumUpByMemberCurrency(IDataReader reader, byte baseCurrency)
        {
            var result = new List<CompanyWinLossItem>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var companyWinloss = new CompanyWinLossItem
                    {
                        Currency = reader.ToString("Currency"),
                        BetCount = reader.ToInt("BetCount"),
                        ActivePlayer = reader.ToInt("ActivePlayer")
                    };

                    GetColumnExtra(reader, companyWinloss);

                    GetCompanyWinLossColumnsBasedOnBaseCurrency(baseCurrency, reader, companyWinloss);

                    result.Add(companyWinloss);
                }
            }

            return result;
        }

        /// <summary>
        /// Get total company win-loss in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Total company win-loss.</returns>
        protected IEnumerable<CompanyWinLossItem> GetCompanyWinLossSumUpBySystemCurrency(IDataReader reader)
        {
            var result = new List<CompanyWinLossItem>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var companyWinloss = new CompanyWinLossItem
                    {
                        BetCount = reader.ToInt("BetCount"),
                        ActivePlayer = reader.ToInt("ActivePlayer")
                    };

                    GetColumnExtra(reader, companyWinloss);

                    GetCompanyWinlossColumns(reader, companyWinloss, SysColSuffix);

                    result.Add(companyWinloss);
                }
            }

            return result;
        }

        /// <summary>
        /// Get company win-loss columns in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="companyWinloss">Company win-loss.</param>
        /// <param name="colSuffix">Column suffix.</param>
        private void GetCompanyWinlossColumns(IDataReader reader, CompanyWinLossItem companyWinloss, string colSuffix)
        {
            SetPropertyValue(reader, companyWinloss, string.Format("TotalTurnover{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("CompanyWinLoss{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("PlayerComm{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("PShare{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("PWin{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("NetGaming{0}", colSuffix));

            GetColumnExtra(reader, companyWinloss, colSuffix);
        }

        /// <summary>
        /// Get company win-loss columns based on base currency.
        /// </summary>
        /// <param name="baseCurrency">Criteria input.</param>
        /// <param name="reader">Data reader.</param>
        /// <param name="companyWinloss">Company win-loss.</param>
        private void GetCompanyWinLossColumnsBasedOnBaseCurrency(byte baseCurrency, IDataReader reader, CompanyWinLossItem companyWinloss)
        {
            const byte MemberCurrency = 0;
            const byte AffiliateCurrency = 2;

            GetCompanyWinlossColumns(reader, companyWinloss, SysColSuffix);

            if (baseCurrency == MemberCurrency)
            {
                GetCompanyWinlossColumns(reader, companyWinloss, string.Empty);
                return;
            }

            if (baseCurrency == AffiliateCurrency)
            {
                GetCompanyWinlossColumns(reader, companyWinloss, AffColSuffix);
            }
        }
    }
}