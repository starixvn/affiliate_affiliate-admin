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
    using Core.Configurations;
    using Core.Utilities;
    using Criterias;
    using Data;
    using DTO;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class ReportDataAccess : IReportDataAccess
    {
        protected const string AffColSuffix = "_Aff";

        protected const string SysColSuffix = "Base";

        /// <summary>
        /// Get data for member win-loss.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Member win-loss.</returns>
        public MemberWinLossReport GetMemberWinLoss(MemberWinlossReportCriteria criteria)
        {
            var memberWinlossReport = new MemberWinLossReport();

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

        #region Common helpers

        /// <summary>
        /// Get output parameters from database.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <param name="objectDB">Object database.</param>
        protected void GetOutputParams(ICriteria criteria, ObjectDb objectDB)
        {
            var outputParams = objectDB.GetOuputParams();

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

        /// <summary>
        /// Set Property Value.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="value">Value Input.</param>
        /// <param name="propertyName">Property Name.</param>
        protected void SetPropertyValue(IDataReader reader, object value, string propertyName)
        {
            value.SetPropertyValue(propertyName, reader.ToDecimal(propertyName));
        }

        #endregion Common helpers

        #region Helpers for member win-loss

        /// <summary>
        /// Get member win-loss list.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Member win-loss list.</returns>
        private IEnumerable<MemberWinLoss> GetMemberWinlossList(IDataReader reader, MemberWinlossReportCriteria criteria)
        {
            var result = new List<MemberWinLoss>();
            const byte GroupByDate = 0;

            while (reader.Read())
            {
                var memberWinloss = new MemberWinLoss
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
        private void GetMemberWinlossColumnsBasedOnBaseCurrency(MemberWinlossReportCriteria criteria, IDataReader reader, MemberWinLoss memberWinloss)
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
        private IEnumerable<MemberWinLoss> GetTotalMemberWinlossInMemberCurrency(IDataReader reader, MemberWinlossReportCriteria criteria)
        {
            var result = new List<MemberWinLoss>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var memberWinloss = new MemberWinLoss
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
        private MemberWinLoss GetTotalMemberWinlossInSystemCurrency(IDataReader reader)
        {
            var playerWinloss = new MemberWinLoss();

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
        private void GetMemberWinlossColumnsInMemberCurrency(IDataReader reader, MemberWinLoss memberWinloss)
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
            memberWinloss.PlayerComm = reader.ToDecimal("PlayerComm");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                memberWinloss.SetPropertyValue(string.Format("{0}Turnover", product), reader.ToDecimal(string.Format("{0}Turnover", product)));
                memberWinloss.SetPropertyValue(string.Format("{0}Winlost", product), reader.ToDecimal(string.Format("{0}Winlost", product)));
                memberWinloss.SetPropertyValue(string.Format("{0}PlayerComm", product), reader.ToDecimal(string.Format("{0}PlayerComm", product)));
            }
        }

        /// <summary>
        /// Get member win-loss columns in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        private void GetMemberWinlossColumnsInSystemCurrency(IDataReader reader, MemberWinLoss memberWinloss)
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
            ////memberWinloss.PlayerCommBase = reader.ToDecimal("PlayerCommBase");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                memberWinloss.SetPropertyValue(string.Format("{0}TurnoverBase", product), reader.ToDouble(string.Format("{0}TurnoverBase", product)));
                memberWinloss.SetPropertyValue(string.Format("{0}WinlostBase", product), reader.ToDouble(string.Format("{0}WinlostBase", product)));
                ////memberWinloss.SetPropertyValue(string.Format("{0}PlayerCommBase", product), reader.ToDouble(string.Format("{0}PlayerCommBase", product)));
            }
        }

        /// <summary>
        /// Get member win-loss columns in affiliate currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        private void GetMemberWinlossColumnsInAffiliateCurrency(IDataReader reader, MemberWinLoss memberWinloss)
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
            ////memberWinloss.PlayerComm_Aff = reader.ToDecimal("PlayerComm_Aff");

            var products = AppSettings.Products.Select(t => t.ProductName.Replace(" ", string.Empty));

            foreach (var product in products)
            {
                memberWinloss.SetPropertyValue(string.Format("{0}Turnover_Aff", product), reader.ToDouble(string.Format("{0}Turnover_Aff", product)));
                memberWinloss.SetPropertyValue(string.Format("{0}Winlost_Aff", product), reader.ToDouble(string.Format("{0}Winlost_Aff", product)));
                ////memberWinloss.SetPropertyValue(string.Format("{0}PlayerComm_Aff", product), reader.ToDouble(string.Format("{0}PlayerComm_Aff", product)));
            }
        }

        #endregion Helpers for member win-loss
    }
}