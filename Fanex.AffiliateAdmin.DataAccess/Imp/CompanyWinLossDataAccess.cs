using System.Collections.Generic;
using System.Data;
using Fanex.AffiliateAdmin.Core.Utilities;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.Common.Extensions;

namespace Fanex.AffiliateAdmin.DataAccess
{
    public abstract class CompanyWinLossDataAccess : ReportDataAccess
    {
        /// <summary>
        /// Get simple company win-loss.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Company win-loss list.</returns>
        protected IEnumerable<CompanyWinloss> GetCompanyWinLossByProductList(IDataReader reader, CompanyWinlossReportCriteria criteria)
        {
            var result = new List<CompanyWinloss>();
            const int GroupByDate = 0;

            while (reader.Read())
            {
                var companyWinloss = new CompanyWinloss
                {
                    WinlostDate = reader.ToDateTime("WinlostDate"),
                    Currency = reader.ToString("Currency"),
                    CurrencyId = reader.ToShort("CurrencyId"),
                    BetCount = reader.ToInt("BetCount"),
                    ActivePlayer = reader.ToInt("ActivePlayer"),
                };

                GetColumnExtra(reader, companyWinloss);

                GetCompanyWinLossColumnsBasedOnBaseCurrency(criteria, reader, companyWinloss);

                if (criteria.GroupBy == GroupByDate)
                {
                    companyWinloss.ExRate = reader.ToDouble("ExRate");
                    companyWinloss.AffExRate = reader.ToDouble("AffExRate");
                }

                result.Add(companyWinloss);
            }

            return result;
        }

        protected virtual void GetColumnExtra(IDataReader reader, CompanyWinloss companyWinloss)
        {
        }

        protected virtual void GetColumnExtra(IDataReader reader, CompanyWinloss companyWinloss, string colSuffix)
        {
        }

        /// <summary>
        /// Get total company win-loss in member currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Total company win-loss list.</returns>
        protected IEnumerable<CompanyWinloss> GetCompanyWinLossSumUpByMemberCurrency(IDataReader reader, CompanyWinlossReportCriteria criteria)
        {
            var result = new List<CompanyWinloss>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var companyWinloss = new CompanyWinloss
                    {
                        Currency = reader.ToString("Currency"),
                        BetCount = reader.ToInt("BetCount"),
                        ActivePlayer = reader.ToInt("ActivePlayer")
                    };

                    GetColumnExtra(reader, companyWinloss);

                    GetCompanyWinLossColumnsBasedOnBaseCurrency(criteria, reader, companyWinloss);

                    result.Add(companyWinloss);
                }
            }

            return result;
        }

        /// <summary>
        /// Get total company win-loss in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria Input.</param>
        /// <returns>Total company win-loss.</returns>
        protected IEnumerable<CompanyWinloss> GetCompanyWinLossSumUpBySystemCurrency(IDataReader reader, CompanyWinlossReportCriteria criteria)
        {
            var result = new List<CompanyWinloss>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var companyWinloss = new CompanyWinloss
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
        /// Get company win-loss columns based on base currency.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <param name="reader">Data reader.</param>
        /// <param name="companyWinloss">Company win-loss.</param>
        private void GetCompanyWinLossColumnsBasedOnBaseCurrency(CompanyWinlossReportCriteria criteria, IDataReader reader, CompanyWinloss companyWinloss)
        {
            const byte MemberCurrency = 0;
            const byte AffiliateCurrency = 2;

            GetCompanyWinlossColumns(reader, companyWinloss, SysColSuffix);

            if (criteria.BaseCurrency == MemberCurrency)
            {
                GetCompanyWinlossColumns(reader, companyWinloss, string.Empty);
                return;
            }

            if (criteria.BaseCurrency == AffiliateCurrency)
            {
                GetCompanyWinlossColumns(reader, companyWinloss, AffColSuffix);
            }
        }

        /// <summary>
        /// Get company win-loss columns in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="companyWinloss">Company win-loss.</param>
        /// <param name="colSuffix">Column suffix.</param>
        /// <param name="viewMode">View mode.</param>
        private void GetCompanyWinlossColumns(IDataReader reader, CompanyWinloss companyWinloss, string colSuffix)
        {
            SetPropertyValue(reader, companyWinloss, string.Format("TotalTurnover{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("TotalWinlost{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("PlayerComm{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("PShare{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("PWin{0}", colSuffix));
            SetPropertyValue(reader, companyWinloss, string.Format("NetGaming{0}", colSuffix));

            GetColumnExtra(reader, companyWinloss, colSuffix);
        }

        protected void SetPropertyValue(IDataReader reader, object obj, string propertyName)
        {
            obj.SetPropertyValue(propertyName, reader.ToDecimal(propertyName));
        }
    }
}