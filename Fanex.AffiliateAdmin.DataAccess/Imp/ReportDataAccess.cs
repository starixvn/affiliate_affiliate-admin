// --------------------------------------------------------------------------
// <copyright file="ReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess.Imp
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Extensions;
    using Criterias;
    using Data;
    using DTO;
    using Fanex.NAP.Framework.Data;
    using Interface;
    using System.Diagnostics;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class ReportDataAccess : IReportDataAccess
    {
        /// <summary>
        /// Get member win-loss.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Player win-loss report object.</returns>
        public MemberWinlossReport GetMemberWinloss(PlayerWinlossReportCriteria criteria)
        {
            var memberWinlossReport = new MemberWinlossReport();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var stopwatch = Stopwatch.StartNew();

                    var reader = objectDb.ExecuteReader(param);

                    stopwatch.Stop();

                    memberWinlossReport.Duration = stopwatch.ElapsedMilliseconds;

                    memberWinlossReport.PlayerWinlossList = GetMemberWinlossList(reader);
                    memberWinlossReport.PlayerWinlossInMemberCurrencies = GetMemberWinlossInMemberCurrencies(reader);
                    memberWinlossReport.PlayerWinlossInSystemCurrency = GetMemberWinlossInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return memberWinlossReport;
        }

        public CompanyWinlossReport GetCompanyWinloss(CompanyWinlossReportCriteria criteria)
        {
            var companyWinlossReport = new CompanyWinlossReport();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var stopwatch = Stopwatch.StartNew();

                    var reader = objectDb.ExecuteReader(param);

                    stopwatch.Stop();

                    companyWinlossReport.Duration = stopwatch.ElapsedMilliseconds;

                    companyWinlossReport.CompanyWinlossList = GetCompanyWinlossList(reader, criteria);
                    companyWinlossReport.CompanyWinlossInMemberCurrencies = GetCompanyWinlossInMemberCurrencies(reader);
                    companyWinlossReport.CompanyWinlossInSystemCurrency = GetCompanyWinlossInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return companyWinlossReport;
        }

        public CommissionReport GetCommission(CommissionReportCriteria criteria)
        {
            var commissionReport = new CommissionReport();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var stopwatch = Stopwatch.StartNew();

                    var reader = objectDb.ExecuteReader(param);

                    stopwatch.Stop();

                    commissionReport.Duration = stopwatch.ElapsedMilliseconds;

                    commissionReport.CommissionList = GetCommissionList(reader, criteria);
                    commissionReport.CommissionInSystemCurrency = GetCommissionInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return commissionReport;
        }

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

        #region Helpers for member win-loss

        /// <summary>
        /// Get member win-loss list.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Member win-loss list.</returns>
        private IEnumerable<MemberWinloss> GetMemberWinlossList(IDataReader reader)
        {
            var result = new List<MemberWinloss>();

            while (reader.Read())
            {
                var memberWinloss = new MemberWinloss
                {
                    WinlostDate = reader.ToDateTime("WinlostDate"),
                    Username = reader.ToString("Username"),
                    Currency = reader.ToString("Currency"),
                    PlayerUsername = reader.ToString("PlayerUsername"),
                    PlayerCurrency = reader.ToString("PlayerCurrency"),
                    ExRate = reader.ToDouble("ExRate"),
                    AffExRate = reader.ToDouble("AffExRate"),
                    BetCount = reader.ToInt("BetCount")
                };

                GetMemberWinlossColumnsRelatedToMoney(reader, memberWinloss);
                result.Add(memberWinloss);
            }

            return result;
        }

        /// <summary>
        /// Get member win-loss columns which related to money.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        private void GetMemberWinlossColumnsRelatedToMoney(IDataReader reader, MemberWinloss memberWinloss)
        {
            GetMemberWinlossColumnsInMemberCurrency(reader, memberWinloss);
            GetMemberWinlossColumnsInSystemCurrency(reader, memberWinloss);
            GetMemberWinlossColumnsInAffiliateCurrency(reader, memberWinloss);
        }

        /// <summary>
        /// Get member win-loss in member currencies.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Member win-loss list.</returns>
        private IEnumerable<MemberWinloss> GetMemberWinlossInMemberCurrencies(IDataReader reader)
        {
            var result = new List<MemberWinloss>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var memberWinloss = new MemberWinloss
                    {
                        PlayerCurrency = reader.ToString("PlayerCurrency")
                    };

                    GetMemberWinlossColumnsRelatedToMoney(reader, memberWinloss);
                    result.Add(memberWinloss);
                }
            }

            return result;
        }

        /// <summary>
        /// Get member win-loss in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Member win-loss object.</returns>
        private MemberWinloss GetMemberWinlossInSystemCurrency(IDataReader reader)
        {
            var playerWinloss = new MemberWinloss();

            if (reader.NextResult())
            {
                reader.Read();
                GetMemberWinlossColumnsInSystemCurrency(reader, playerWinloss);
            }

            return playerWinloss;
        }

        /// <summary>
        /// Get member Win-loss columns which have values in member currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="playerWinloss">Player win-loss.</param>
        private void GetMemberWinlossColumnsInMemberCurrency(IDataReader reader, MemberWinloss playerWinloss)
        {
            playerWinloss.Deposit = reader.ToDecimal("Deposit");
            playerWinloss.Withdrawal = reader.ToDecimal("Withdrawal");
            playerWinloss.Bonus = reader.ToDecimal("Bonus");
            playerWinloss.Adjustment = reader.ToDecimal("Adjustment");
            playerWinloss.PSP = reader.ToDecimal("PSP");
            playerWinloss.SportsbookTurnover = reader.ToDecimal("SportsbookTurnover");
            playerWinloss.NumberGameTurnover = reader.ToDecimal("NumberGameTurnover");
            playerWinloss.LiveCasinoTurnover = reader.ToDecimal("LiveCasinoTurnover");
            playerWinloss.RNGCasinoTurnover = reader.ToDecimal("RNGCasinoTurnover");
            playerWinloss.TotalTurnover = reader.ToDecimal("TotalTurnover");
            playerWinloss.SportsbookWinlost = reader.ToDecimal("SportsbookWinlost");
            playerWinloss.NumberGameWinlost = reader.ToDecimal("NumberGameWinlost");
            playerWinloss.LiveCasinoWinlost = reader.ToDecimal("LiveCasinoWinlost");
            playerWinloss.RNGCasinoWinlost = reader.ToDecimal("RNGCasinoWinlost");
            playerWinloss.TotalWinlost = reader.ToDecimal("TotalWinlost");
        }

        /// <summary>
        /// Get member Win-loss columns which have values in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="playerWinloss">Player win-loss.</param>
        private void GetMemberWinlossColumnsInSystemCurrency(IDataReader reader, MemberWinloss playerWinloss)
        {
            // base
            playerWinloss.DepositBase = reader.ToDouble("DepositBase");
            playerWinloss.WithdrawalBase = reader.ToDouble("WithdrawalBase");
            playerWinloss.BonusBase = reader.ToDouble("BonusBase");
            playerWinloss.AdjustmentBase = reader.ToDouble("AdjustmentBase");
            playerWinloss.PSPBase = reader.ToDouble("PSPBase");
            playerWinloss.SportsbookTurnoverBase = reader.ToDouble("SportsbookTurnoverBase");
            playerWinloss.NumberGameTurnoverBase = reader.ToDouble("NumberGameTurnoverBase");
            playerWinloss.LiveCasinoTurnoverBase = reader.ToDouble("LiveCasinoTurnoverBase");
            playerWinloss.RNGCasinoTurnoverBase = reader.ToDouble("RNGCasinoTurnoverBase");
            playerWinloss.TotalTurnoverBase = reader.ToDouble("TotalTurnoverBase");
            playerWinloss.SportsbookWinlostBase = reader.ToDouble("SportsbookWinlostBase");
            playerWinloss.NumberGameWinlostBase = reader.ToDouble("NumberGameWinlostBase");
            playerWinloss.LiveCasinoWinlostBase = reader.ToDouble("LiveCasinoWinlostBase");
            playerWinloss.RNGCasinoWinlostBase = reader.ToDouble("RNGCasinoWinlostBase");
            playerWinloss.TotalWinlostBase = reader.ToDouble("TotalWinlostBase");
        }

        /// <summary>
        /// Get member Win-loss columns which have values in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="playerWinloss">Player win-loss.</param>
        private void GetMemberWinlossColumnsInAffiliateCurrency(IDataReader reader, MemberWinloss playerWinloss)
        {
            playerWinloss.Deposit_Aff = reader.ToDouble("Deposit_Aff");
            playerWinloss.Withdrawal_Aff = reader.ToDouble("Withdrawal_Aff");
            playerWinloss.Bonus_Aff = reader.ToDouble("Bonus_Aff");
            playerWinloss.Adjustment_Aff = reader.ToDouble("Adjustment_Aff");
            playerWinloss.PSP_Aff = reader.ToDouble("PSP_Aff");
            playerWinloss.SportsbookTurnover_Aff = reader.ToDouble("SportsbookTurnover_Aff");
            playerWinloss.NumberGameTurnover_Aff = reader.ToDouble("NumberGameTurnover_Aff");
            playerWinloss.LiveCasinoTurnover_Aff = reader.ToDouble("LiveCasinoTurnover_Aff");
            playerWinloss.RNGCasinoTurnover_Aff = reader.ToDouble("RNGCasinoTurnover_Aff");
            playerWinloss.TotalTurnover_Aff = reader.ToDouble("TotalTurnover_Aff");
            playerWinloss.SportsbookWinlost_Aff = reader.ToDouble("SportsbookWinlost_Aff");
            playerWinloss.NumberGameWinlost_Aff = reader.ToDouble("NumberGameWinlost_Aff");
            playerWinloss.LiveCasinoWinlost_Aff = reader.ToDouble("LiveCasinoWinlost_Aff");
            playerWinloss.RNGCasinoWinlost_Aff = reader.ToDouble("RNGCasinoWinlost_Aff");
            playerWinloss.TotalWinlost_Aff = reader.ToDouble("TotalWinlost_Aff");
        }

        #endregion

        #region Helpers for company win-loss

        private IEnumerable<CompanyWinloss> GetCompanyWinlossList(IDataReader reader, CompanyWinlossReportCriteria criteria)
        {
            var result = new List<CompanyWinloss>();

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

                if (criteria.GroupBy == GroupByDate)
                {
                    companyWinloss.ExRate = reader.ToDouble("ExRate");
                    companyWinloss.AffExRate = reader.ToDouble("AffExRate");
                }

                GetCompanyWinlossColumnsRelatedToMoney(reader, companyWinloss);
                result.Add(companyWinloss);
            }

            return result;
        }

        private IEnumerable<CompanyWinloss> GetCompanyWinlossInMemberCurrencies(IDataReader reader)
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

                    GetCompanyWinlossColumnsRelatedToMoney(reader, companyWinloss);
                    result.Add(companyWinloss);
                }
            }

            return result;
        }

        private CompanyWinloss GetCompanyWinlossInSystemCurrency(IDataReader reader)
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

        private void GetCompanyWinlossColumnsRelatedToMoney(IDataReader reader, CompanyWinloss companyWinloss)
        {
            GetCompanyWinlossColumnsInMemberCurrency(reader, companyWinloss);
            GetCompanyWinlossColumnsInSystemCurrency(reader, companyWinloss);
            GetCompanyWinlossColumnsInAffiliateCurrency(reader, companyWinloss);
        }

        private void GetCompanyWinlossColumnsInAffiliateCurrency(IDataReader reader, CompanyWinloss companyWinloss)
        {
            companyWinloss.Deposit_Aff = reader.ToDecimal("Deposit_Aff");
            companyWinloss.Withdrawal_Aff = reader.ToDecimal("Withdrawal_Aff");
            companyWinloss.Bonus_Aff = reader.ToDecimal("Bonus_Aff");
            companyWinloss.Adjustment_Aff = reader.ToDecimal("Adjustment_Aff");
            companyWinloss.PSP_Aff = reader.ToDecimal("PSP_Aff");
            companyWinloss.SportsbookTurnover_Aff = reader.ToDecimal("SportsbookTurnover_Aff");
            companyWinloss.NumberGameTurnover_Aff = reader.ToDecimal("NumberGameTurnover_Aff");
            companyWinloss.LiveCasinoTurnover_Aff = reader.ToDecimal("LiveCasinoTurnover_Aff");
            companyWinloss.RNGCasinoTurnover_Aff = reader.ToDecimal("RNGCasinoTurnover_Aff");
            companyWinloss.TotalTurnover_Aff = reader.ToDecimal("TotalTurnover_Aff"); // Total Turnover.
            companyWinloss.TotalMemberWinlost_Aff = reader.ToDecimal("TotalWinlost_Aff");
            companyWinloss.SportsbookWinlost_Aff = reader.ToDecimal("SportsbookWinlost_Aff");
            companyWinloss.NumberGameWinlost_Aff = reader.ToDecimal("NumberGameWinlost_Aff");
            companyWinloss.LiveCasinoWinlost_Aff = reader.ToDecimal("LiveCasinoWinlost_Aff");
            companyWinloss.RNGCasinoWinlost_Aff = reader.ToDecimal("RNGCasinoWinlost_Aff");
        }

        /// <summary>
        /// Get company win-loss columns in system currency.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="companyWinloss"></param>
        private void GetCompanyWinlossColumnsInSystemCurrency(IDataReader reader, CompanyWinloss companyWinloss)
        {
            companyWinloss.DepositBase = reader.ToDecimal("DepositBase");
            companyWinloss.WithdrawalBase = reader.ToDecimal("WithdrawalBase");
            companyWinloss.BonusBase = reader.ToDecimal("BonusBase");
            companyWinloss.AdjustmentBase = reader.ToDecimal("AdjustmentBase");
            companyWinloss.PSPBase = reader.ToDecimal("PSPBase");
            companyWinloss.SportsbookTurnoverBase = reader.ToDecimal("SportsbookTurnoverBase");
            companyWinloss.NumberGameTurnoverBase = reader.ToDecimal("NumberGameTurnoverBase");
            companyWinloss.LiveCasinoTurnoverBase = reader.ToDecimal("LiveCasinoTurnoverBase");
            companyWinloss.RNGCasinoTurnoverBase = reader.ToDecimal("RNGCasinoTurnoverBase");
            companyWinloss.TotalTurnoverBase = reader.ToDecimal("TotalTurnoverBase"); // Total Turnover.
            companyWinloss.TotalMemberWinlostBase = reader.ToDecimal("TotalWinlostBase");
            companyWinloss.SportsbookWinlostBase = reader.ToDecimal("SportsbookWinlostBase");
            companyWinloss.NumberGameWinlostBase = reader.ToDecimal("NumberGameWinlostBase");
            companyWinloss.LiveCasinoWinlostBase = reader.ToDecimal("LiveCasinoWinlostBase");
            companyWinloss.RNGCasinoWinlostBase = reader.ToDecimal("RNGCasinoWinlostBase");
        }

        private void GetCompanyWinlossColumnsInMemberCurrency(IDataReader reader, CompanyWinloss companyWinloss)
        {
            companyWinloss.Deposit = reader.ToDecimal("Deposit");
            companyWinloss.Withdrawal = reader.ToDecimal("Withdrawal");
            companyWinloss.Bonus = reader.ToDecimal("Bonus");
            companyWinloss.Adjustment = reader.ToDecimal("Adjustment");
            companyWinloss.PSP = reader.ToDecimal("PSP");
            companyWinloss.SportsbookTurnover = reader.ToDecimal("SportsbookTurnover");
            companyWinloss.NumberGameTurnover = reader.ToDecimal("NumberGameTurnover");
            companyWinloss.LiveCasinoTurnover = reader.ToDecimal("LiveCasinoTurnover");
            companyWinloss.RNGCasinoTurnover = reader.ToDecimal("RNGCasinoTurnover");
            companyWinloss.TotalTurnover = reader.ToDecimal("TotalTurnover"); // Total Turnover.
            companyWinloss.TotalMemberWinlost = reader.ToDecimal("TotalWinlost");
            companyWinloss.SportsbookWinlost = reader.ToDecimal("SportsbookWinlost");
            companyWinloss.NumberGameWinlost = reader.ToDecimal("NumberGameWinlost");
            companyWinloss.LiveCasinoWinlost = reader.ToDecimal("LiveCasinoWinlost");
            companyWinloss.RNGCasinoWinlost = reader.ToDecimal("RNGCasinoWinlost");
        } 

        #endregion

        #region Helpers for commission

        private IEnumerable<Commission> GetCommissionList(IDataReader reader, CommissionReportCriteria criteria)
        {
            var result = new List<Commission>();

            while (reader.Read())
            {
                var commission = new Commission
                {
                    AffId = reader.ToInt("AffId"),
                    Status = reader.ToString("STATUS"),
                    Username = reader.ToString("Username"),
                    AffType = reader.ToString("AffType"),
                    Language = reader.ToString("LANGUAGE"),
                    Currency = reader.ToString("Currency"),
                    GroupID = reader.ToInt("GroupID"),
                    GroupName = reader.ToString("GroupName"),

                    WinlostDate = reader.ToDateTime("WinlostDate"),
                    Currency = reader.ToString("Currency"),
                    CurrencyId = reader.ToShort("CurrencyId"),
                    BetCount = reader.ToInt("BetCount"),
                    ActivePlayer = reader.ToInt("ActivePlayer")
                };

                if (criteria.GroupBy == 0)
                {
                    commission.ExRate = reader.ToDouble("ExRate");
                    commission.AffExRate = reader.ToDouble("AffExRate");
                }

                GetCompanyWinlossColumnsRelatedToMoney(reader, commission);
                result.Add(commission);
            }

            return result;
        }

        private IEnumerable<Commission> GetCommissionInSystemCurrency(IDataReader reader)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
