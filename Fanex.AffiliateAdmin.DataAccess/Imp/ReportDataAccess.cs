// --------------------------------------------------------------------------
// <copyright file="ReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Extensions;
    using Criterias;
    using Data;
    using Core.Configurations;
    using Core.Utilities;
    using DTO;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class ReportDataAccess : IReportDataAccess
    {
        /// <summary>
        /// Get data for member win-loss.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Member win-loss.</returns>
        public MemberWinlossReport GetMemberWinloss(MemberWinlossReportCriteria criteria)
        {
            var memberWinlossReport = new MemberWinlossReport();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var reader = objectDb.ExecuteReader(param);

                    memberWinlossReport.MemberWinlossList = GetMemberWinlossList(reader, criteria);
                    memberWinlossReport.TotalMemberWinlossInMemberCurrencies = GetTotalMemberWinlossInMemberCurrency(reader, criteria);
                    memberWinlossReport.TotalMemberWinlossInSystemCurrency = GetTotalMemberWinlossInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return memberWinlossReport;
        }

        /// <summary>
        /// Get data for company win-loss.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Company win-loss report.</returns>
        public CompanyWinlossReport GetCompanyWinloss(CompanyWinlossReportCriteria criteria)
        {
            var companyWinlossReport = new CompanyWinlossReport();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var reader = objectDb.ExecuteReader(param);

                    companyWinlossReport.CompanyWinlossList = GetCompanyWinlossList(reader, criteria);
                    companyWinlossReport.TotalCompanyWinlossInMemberCurrencies = GetTotalCompanyWinlossInMemberCurrency(reader, criteria);
                    companyWinlossReport.TotalCompanyWinlossInSystemCurrency = GetTotalCompanyWinlossInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return companyWinlossReport;
        }

        /// <summary>
        /// Get data for commission report.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Commission report.</returns>
        public CommissionReport GetCommission(CommissionReportCriteria criteria)
        {
            var commissionReport = new CommissionReport();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var reader = objectDb.ExecuteReader(param);

                    commissionReport.CommissionList = GetCommissionList(reader, criteria);
                    commissionReport.CommissionInAffiliateCurrency = GetTotalCommissionInAffiliateCurrency(reader, criteria);
                    commissionReport.CommissionInSystemCurrency = GetTotalCommissionInSystemCurrency(reader, criteria);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return commissionReport;
        }

        #region Common helpers

        /// <summary>
        /// Get output parameters from database.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <param name="objectDb">Object database.</param>
        private void GetOutputParams(ICriteria criteria, ObjectDb objectDb)
        {
            var outputParams = objectDb.GetOuputParams();

            if (outputParams != null)
            {
                var properties = criteria.GetType().GetProperties();

                foreach (string outputParam in outputParams.Keys)
                {
                    foreach (var property in properties)
                    {
                        if (property.CanWrite && property.Name.ToLower() == outputParam)
                        {
                            property.SetValue(criteria, outputParams[outputParam], null);
                        }
                    }
                }
            }
        }

        #endregion Common helpers

        #region Helpers for member win-loss

        /// <summary>
        /// Get member win-loss list.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Member win-loss list.</returns>
        private IEnumerable<MemberWinloss> GetMemberWinlossList(IDataReader reader, MemberWinlossReportCriteria criteria)
        {
            var result = new List<MemberWinloss>();
            const byte GroupByDate = 0;

            while (reader.Read())
            {
                var memberWinloss = new MemberWinloss
                {
                    Username = reader.ToString("Username"),
                    Currency = reader.ToString("Currency"),
                    PlayerUsername = reader.ToString("PlayerUsername"),
                    PlayerCurrency = reader.ToString("PlayerCurrency"),
                    BetCount = reader.ToInt("BetCount"),
                };

                if (criteria.GroupBy == GroupByDate)
                {
                    memberWinloss.WinlostDate = reader.ToDateTime("WinlostDate");
                    memberWinloss.ExRate = reader.ToDouble("ExRate");
                    memberWinloss.AffExRate = reader.ToDouble("AffExRate");
                }

                GetMemberWinlossColumnsBasedOnBaseCurrency(criteria, reader, memberWinloss);

                result.Add(memberWinloss);
            }

            return result;
        }

        /// <summary>
        /// Get member win-loss columns based on base currency.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        private void GetMemberWinlossColumnsBasedOnBaseCurrency(MemberWinlossReportCriteria criteria, IDataReader reader, MemberWinloss memberWinloss)
        {
            const byte MemberCurrency = 0;
            const byte AffiliateCurrency = 2;

            GetMemberWinlossColumnsInSystemCurrency(reader, memberWinloss);
          
            if (criteria.BaseCurrency == MemberCurrency)
            {
                GetMemberWinlossColumnsInMemberCurrency(reader, memberWinloss);
                return;
            }

            if (criteria.BaseCurrency == AffiliateCurrency)
            {
                GetMemberWinlossColumnsInAffiliateCurrency(reader, memberWinloss);
                return;
            }
        }

        /// <summary>
        /// Get total member win-loss in member currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Total member win-loss list.</returns>
        private IEnumerable<MemberWinloss> GetTotalMemberWinlossInMemberCurrency(IDataReader reader, MemberWinlossReportCriteria criteria)
        {
            var result = new List<MemberWinloss>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var memberWinloss = new MemberWinloss
                    {
                        PlayerCurrency = reader.ToString("PlayerCurrency"),
                        BetCount = reader.ToInt("BetCount")
                    };

                    GetMemberWinlossColumnsBasedOnBaseCurrency(criteria, reader, memberWinloss);

                    result.Add(memberWinloss);
                }
            }

            return result;
        }

        /// <summary>
        /// Get total member win-loss in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Total member win-loss.</returns>
        private MemberWinloss GetTotalMemberWinlossInSystemCurrency(IDataReader reader)
        {
            var playerWinloss = new MemberWinloss();

            if (reader.NextResult())
            {
                reader.Read();
                playerWinloss.BetCount = reader.ToInt("BetCount");
                GetMemberWinlossColumnsInSystemCurrency(reader, playerWinloss);
            }

            return playerWinloss;
        }

        /// <summary>
        /// Get member win-loss columns in member currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        private void GetMemberWinlossColumnsInMemberCurrency(IDataReader reader, MemberWinloss memberWinloss)
        {
            memberWinloss.Deposit = reader.ToDecimal("Deposit");
            memberWinloss.Withdrawal = reader.ToDecimal("Withdrawal");
            memberWinloss.Bonus = reader.ToDecimal("Bonus");
            memberWinloss.Adjustment = reader.ToDecimal("Adjustment");
            memberWinloss.PSP = reader.ToDecimal("PSP");
            memberWinloss.TotalTurnover = reader.ToDecimal("TotalTurnover");
            memberWinloss.TotalWinlost = reader.ToDecimal("TotalWinlost");
            memberWinloss.PShare = reader.ToDecimal("PShare");
            memberWinloss.PWin = reader.ToDecimal("PWin");
            memberWinloss.NetGaming = reader.ToDecimal("NetGaming");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                memberWinloss.SetPropertyValue(string.Format("{0}Turnover", product), reader.ToDecimal(string.Format("{0}Turnover", product)));
                memberWinloss.SetPropertyValue(string.Format("{0}Winlost", product), reader.ToDecimal(string.Format("{0}Winlost", product)));
            }
        }

        /// <summary>
        /// Get member win-loss columns in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        private void GetMemberWinlossColumnsInSystemCurrency(IDataReader reader, MemberWinloss memberWinloss)
        {
            memberWinloss.DepositBase = reader.ToDouble("DepositBase");
            memberWinloss.WithdrawalBase = reader.ToDouble("WithdrawalBase");
            memberWinloss.BonusBase = reader.ToDouble("BonusBase");
            memberWinloss.AdjustmentBase = reader.ToDouble("AdjustmentBase");
            memberWinloss.PSPBase = reader.ToDouble("PSPBase");
            memberWinloss.TotalTurnoverBase = reader.ToDouble("TotalTurnoverBase");
            memberWinloss.TotalWinlostBase = reader.ToDouble("TotalWinlostBase");
            memberWinloss.PShareBase = reader.ToDecimal("PShareBase");
            memberWinloss.PWinBase = reader.ToDecimal("PWinBase");
            memberWinloss.NetGamingBase = reader.ToDecimal("NetGamingBase");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                memberWinloss.SetPropertyValue(string.Format("{0}TurnoverBase", product), reader.ToDouble(string.Format("{0}TurnoverBase", product)));
                memberWinloss.SetPropertyValue(string.Format("{0}WinlostBase", product), reader.ToDouble(string.Format("{0}WinlostBase", product)));
            }
        }

        /// <summary>
        /// Get member win-loss columns in affiliate currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        private void GetMemberWinlossColumnsInAffiliateCurrency(IDataReader reader, MemberWinloss memberWinloss)
        {
            memberWinloss.Deposit_Aff = reader.ToDouble("Deposit_Aff");
            memberWinloss.Withdrawal_Aff = reader.ToDouble("Withdrawal_Aff");
            memberWinloss.Bonus_Aff = reader.ToDouble("Bonus_Aff");
            memberWinloss.Adjustment_Aff = reader.ToDouble("Adjustment_Aff");
            memberWinloss.PSP_Aff = reader.ToDouble("PSP_Aff");
            memberWinloss.TotalTurnover_Aff = reader.ToDouble("TotalTurnover_Aff");
            memberWinloss.TotalWinlost_Aff = reader.ToDouble("TotalWinlost_Aff");
            memberWinloss.PShare_Aff = reader.ToDecimal("PShare_Aff");
            memberWinloss.PWin_Aff = reader.ToDecimal("PWin_Aff");
            memberWinloss.NetGaming_Aff = reader.ToDecimal("NetGaming_Aff");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                memberWinloss.SetPropertyValue(string.Format("{0}Turnover_Aff", product), reader.ToDouble(string.Format("{0}Turnover_Aff", product)));
                memberWinloss.SetPropertyValue(string.Format("{0}Winlost_Aff", product), reader.ToDouble(string.Format("{0}Winlost_Aff", product)));
            }
        }

        #endregion Helpers for member win-loss

        #region Helpers for company win-loss

        /// <summary>
        /// Get simple company win-loss.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Company win-loss list.</returns>
        private IEnumerable<CompanyWinloss> GetCompanyWinlossList(IDataReader reader, CompanyWinlossReportCriteria criteria)
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
                    ActivePlayer = reader.ToInt("ActivePlayer")
                };

                GetCompanyWinlossColumnsBasedOnBaseCurrency(criteria, reader, companyWinloss);

                if (criteria.GroupBy == GroupByDate)
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
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Total company win-loss list.</returns>
        private IEnumerable<CompanyWinloss> GetTotalCompanyWinlossInMemberCurrency(IDataReader reader, CompanyWinlossReportCriteria criteria)
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

                    GetCompanyWinlossColumnsBasedOnBaseCurrency(criteria, reader, companyWinloss);

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
        private CompanyWinloss GetTotalCompanyWinlossInSystemCurrency(IDataReader reader)
        {
            var companyWinloss = new CompanyWinloss();

            if (reader.NextResult())
            {
                reader.Read();
                companyWinloss.BetCount = reader.ToInt("BetCount");
                companyWinloss.ActivePlayer = reader.ToInt("ActivePlayer");

                GetCompanyWinlossColumnsInSystemCurrency(reader, companyWinloss);
            }

            return companyWinloss;
        }

        /// <summary>
        /// Get company win-loss columns based on base currency.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <param name="reader">Data reader.</param>
        /// <param name="companyWinloss">Company win-loss.</param>
        private void GetCompanyWinlossColumnsBasedOnBaseCurrency(CompanyWinlossReportCriteria criteria, IDataReader reader, CompanyWinloss companyWinloss)
        {
            const byte MemberCurrency = 0;
            const byte AffiliateCurrency = 2;

            GetCompanyWinlossColumnsInSystemCurrency(reader, companyWinloss);

            if (criteria.BaseCurrency == MemberCurrency)
            {
                GetCompanyWinlossColumnsInMemberCurrency(reader, companyWinloss);
                return;
            }

            if (criteria.BaseCurrency == AffiliateCurrency)
            {
                GetCompanyWinlossColumnsInAffiliateCurrency(reader, companyWinloss);
            }
        }

        /// <summary>
        /// Get company win-loss columns in affiliate currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="companyWinloss">Company win-loss.</param>
        private void GetCompanyWinlossColumnsInAffiliateCurrency(IDataReader reader, CompanyWinloss companyWinloss)
        {
            companyWinloss.Deposit_Aff = reader.ToDecimal("Deposit_Aff");
            companyWinloss.Withdrawal_Aff = reader.ToDecimal("Withdrawal_Aff");
            companyWinloss.Bonus_Aff = reader.ToDecimal("Bonus_Aff");
            companyWinloss.Adjustment_Aff = reader.ToDecimal("Adjustment_Aff");
            companyWinloss.PSP_Aff = reader.ToDecimal("PSP_Aff");
            companyWinloss.TotalTurnover_Aff = reader.ToDecimal("TotalTurnover_Aff");
            companyWinloss.TotalMemberWinlost_Aff = reader.ToDecimal("TotalWinlost_Aff");
            companyWinloss.PShare_Aff = reader.ToDecimal("PShare_Aff");
            companyWinloss.PWin_Aff = reader.ToDecimal("PWin_Aff");
            companyWinloss.NetGaming_Aff = reader.ToDecimal("NetGaming_Aff");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                companyWinloss.SetPropertyValue(string.Format("{0}Turnover_Aff", product), reader.ToDecimal(string.Format("{0}Turnover_Aff", product)));
                companyWinloss.SetPropertyValue(string.Format("{0}Winlost_Aff", product), reader.ToDecimal(string.Format("{0}Winlost_Aff", product)));
            }
        }

        /// <summary>
        /// Get company win-loss columns in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="companyWinloss">Company win-loss.</param>
        private void GetCompanyWinlossColumnsInSystemCurrency(IDataReader reader, CompanyWinloss companyWinloss)
        {
            companyWinloss.DepositBase = reader.ToDecimal("DepositBase");
            companyWinloss.WithdrawalBase = reader.ToDecimal("WithdrawalBase");
            companyWinloss.BonusBase = reader.ToDecimal("BonusBase");
            companyWinloss.AdjustmentBase = reader.ToDecimal("AdjustmentBase");
            companyWinloss.PSPBase = reader.ToDecimal("PSPBase");
            companyWinloss.TotalTurnoverBase = reader.ToDecimal("TotalTurnoverBase");
            companyWinloss.TotalMemberWinlostBase = reader.ToDecimal("TotalWinlostBase");
            companyWinloss.PShareBase = reader.ToDecimal("PShareBase");
            companyWinloss.PWinBase = reader.ToDecimal("PWinBase");
            companyWinloss.NetGamingBase = reader.ToDecimal("NetGamingBase");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                companyWinloss.SetPropertyValue(string.Format("{0}TurnoverBase", product), reader.ToDecimal(string.Format("{0}TurnoverBase", product)));
                companyWinloss.SetPropertyValue(string.Format("{0}WinlostBase", product), reader.ToDecimal(string.Format("{0}WinlostBase", product)));
            }
        }

        /// <summary>
        /// Get company win-loss columns in member currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="companyWinloss">Company win-loss.</param>
        private void GetCompanyWinlossColumnsInMemberCurrency(IDataReader reader, CompanyWinloss companyWinloss)
        {
            companyWinloss.Deposit = reader.ToDecimal("Deposit");
            companyWinloss.Withdrawal = reader.ToDecimal("Withdrawal");
            companyWinloss.Bonus = reader.ToDecimal("Bonus");
            companyWinloss.Adjustment = reader.ToDecimal("Adjustment");
            companyWinloss.PSP = reader.ToDecimal("PSP");
            companyWinloss.TotalTurnover = reader.ToDecimal("TotalTurnover");
            companyWinloss.TotalMemberWinlost = reader.ToDecimal("TotalWinlost");
            companyWinloss.PShare = reader.ToDecimal("PShare");
            companyWinloss.PWin = reader.ToDecimal("PWin");
            companyWinloss.NetGaming = reader.ToDecimal("NetGaming");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                companyWinloss.SetPropertyValue(string.Format("{0}Turnover", product), reader.ToDecimal(string.Format("{0}Turnover", product)));
                companyWinloss.SetPropertyValue(string.Format("{0}Winlost", product), reader.ToDecimal(string.Format("{0}Winlost", product)));
            }
        }

        #endregion Helpers for company win-loss

        #region Helpers for commission

        /// <summary>
        /// Get simple commission list.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Commission list.</returns>
        private IEnumerable<CommissionItem> GetCommissionList(IDataReader reader, CommissionReportCriteria criteria)
        {
            var result = new List<CommissionItem>();

            while (reader.Read())
            {
                var commission = new CommissionItem
                {
                    AffId = reader.ToInt("AffId"),
                    Status = reader.ToString("STATUS"),
                    Username = reader.ToString("Username"),
                    AffType = reader.ToString("AffType"),
                    Language = reader.ToString("LANGUAGE"),
                    Currency = reader.ToString("Currency"),
                    DeductedPercentage = reader.ToDecimal("DeductedPercentage")
                };

                GetCommissionColumnsNotRelatedToCurrency(reader, commission);

                GetCommissionColumnsBasedOnBaseCurrency(criteria, reader, commission);

                if (IsGroupByMonth(criteria))
                {
                    commission.WinlostDate = reader.ToDateTime("WinlostDate");
                    commission.GroupID = reader.ToInt("GroupID");
                    commission.GroupName = reader.ToString("GroupName");

                    GetAdditionalCommissionInfo(reader, commission);
                }

                result.Add(commission);
            }

            return result;
        }

        /// <summary>
        /// Get total commission in affiliate currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Total commission list.</returns>
        private IEnumerable<CommissionItem> GetTotalCommissionInAffiliateCurrency(IDataReader reader, CommissionReportCriteria criteria)
        {
            var result = new List<CommissionItem>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var commission = new CommissionItem { Currency = reader.ToString("Currency") };

                    GetCommissionColumnsNotRelatedToCurrency(reader, commission);

                    GetCommissionColumnsBasedOnBaseCurrency(criteria, reader, commission);

                    if (IsGroupByMonth(criteria))
                    {
                        GetAdditionalCommissionInfo(reader, commission);
                    }

                    result.Add(commission);
                }
            }

            return result;
        }

        /// <summary>
        /// Get total commission in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Total commission.</returns>
        private CommissionItem GetTotalCommissionInSystemCurrency(IDataReader reader, CommissionReportCriteria criteria)
        {
            var commission = new CommissionItem();

            if (reader.NextResult())
            {
                reader.Read();

                GetCommissionColumnsNotRelatedToCurrency(reader, commission);

                GetCommissionColumnsInSystemCurrency(reader, commission);

                if (IsGroupByMonth(criteria))
                {
                    GetAdditionalCommissionInfo(reader, commission);
                }
            }

            return commission;
        }

        /// <summary>
        /// Whether commission report is group by month or not.
        /// </summary>
        /// <param name="criteria">Commission report criteria.</param>
        /// <returns>True if grouped by month.</returns>
        private bool IsGroupByMonth(CommissionReportCriteria criteria)
        {
            const int GroupByMonth = 0;

            return criteria.GroupBy == GroupByMonth;
        }

        /// <summary>
        /// Get commission columns which not related to currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="commission">Commission object.</param>
        private void GetCommissionColumnsNotRelatedToCurrency(IDataReader reader, CommissionItem commission)
        {
            commission.BetCount = reader.ToInt("BetCount");
            commission.ActivePlayers = reader.ToInt("ActivePlayers");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                commission.SetPropertyValue(string.Format("{0}ActivePlayers", product), reader.ToInt(string.Format("{0}ActivePlayers", product)));
                commission.SetPropertyValue(string.Format("{0}BetCount", product), reader.ToInt(string.Format("{0}BetCount", product)));
            }
        }

        /// <summary>
        /// Get commission columns based on base currency.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <param name="reader">Data reader.</param>
        /// <param name="commission">Commission object.</param>
        private void GetCommissionColumnsBasedOnBaseCurrency(CommissionReportCriteria criteria, IDataReader reader, CommissionItem commission)
        {
            const byte AffiliateCurrency = 0;
            const byte SystemCurrency = 1;

            if (criteria.BaseCurrency == AffiliateCurrency)
            {
                GetCommissionColumnsInAffiliateCurrency(reader, commission);
                return;
            }

            if (criteria.BaseCurrency == SystemCurrency)
            {
                GetCommissionColumnsInSystemCurrency(reader, commission);
                return;
            }
        }

        /// <summary>
        /// Get commission columns in affiliate currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="commission">Commission object.</param>
        private void GetCommissionColumnsInAffiliateCurrency(IDataReader reader, CommissionItem commission)
        {
            commission.Commission = reader.ToDecimal("Commission");
            commission.Deposit = reader.ToDecimal("Deposit");
            commission.Withdrawal = reader.ToDecimal("Withdrawal");
            commission.Turnover = reader.ToDecimal("Turnover");
            commission.Winloss = reader.ToDecimal("Winloss");
            commission.Cashback = reader.ToDecimal("Cashback");
            commission.PShare = reader.ToDecimal("PShare");
            commission.PWin = reader.ToDecimal("PWin");
            commission.NetGaming = reader.ToDecimal("NetGaming");

            // Columns by product.
            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                commission.SetPropertyValue(string.Format("{0}Winloss", product), reader.ToDecimal(string.Format("{0}Winloss", product)));
                commission.SetPropertyValue(string.Format("{0}Commission", product), reader.ToDecimal(string.Format("{0}Commission", product)));
                commission.SetPropertyValue(string.Format("{0}Turnover", product), reader.ToDecimal(string.Format("{0}Turnover", product)));
            }
        }

        /// <summary>
        /// Get commission columns in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="commission">Commission object.</param>
        private void GetCommissionColumnsInSystemCurrency(IDataReader reader, CommissionItem commission)
        {
            commission.CommissionBase = reader.ToDecimal("CommissionBase");
            commission.DepositBase = reader.ToDecimal("DepositBase");
            commission.WithdrawalBase = reader.ToDecimal("WithdrawalBase");
            commission.TurnoverBase = reader.ToDecimal("TurnoverBase");
            commission.WinlossBase = reader.ToDecimal("WinlossBase");
            commission.CashbackBase = reader.ToDecimal("CashbackBase");
            commission.PShareBase = reader.ToDecimal("PShareBase");
            commission.PWinBase = reader.ToDecimal("PWinBase");
            commission.NetGamingBase = reader.ToDecimal("NetGamingBase");

            // Columns by product.
            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                commission.SetPropertyValue(string.Format("{0}WinlossBase", product), reader.ToDecimal(string.Format("{0}WinlossBase", product)));
                commission.SetPropertyValue(string.Format("{0}CommissionBase", product), reader.ToDecimal(string.Format("{0}CommissionBase", product)));
                commission.SetPropertyValue(string.Format("{0}TurnoverBase", product), reader.ToDecimal(string.Format("{0}TurnoverBase", product)));
            }
        }

        /// <summary>
        /// Get additional commission info.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="commission">Commission object.</param>
        private void GetAdditionalCommissionInfo(IDataReader reader, CommissionItem commission)
        {
            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                commission.SetPropertyValue(string.Format("{0}QM", product), reader.ToInt(string.Format("{0}QM", product)));
                commission.SetPropertyValue(string.Format("{0}CommissionPercent", product), reader.ToDecimal(string.Format("{0}CommissionPercent", product)));
            }
        }

        #endregion Helpers for commission
    }
}