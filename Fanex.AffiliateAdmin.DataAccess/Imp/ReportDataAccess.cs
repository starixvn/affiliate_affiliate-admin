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
        public PlayerWinlossReport GetMemberWinloss(PlayerWinlossReportCriteria criteria)
        {
            var playerWinlossReport = new PlayerWinlossReport();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var reader = objectDb.ExecuteReader(param);

                    playerWinlossReport.PlayerWinlossList = GetMemberWinlossList(reader);
                    playerWinlossReport.PlayerWinlossInMemberCurrencies = GetMemberWinlossInMemberCurrencies(reader);
                    playerWinlossReport.PlayerWinlossInSystemCurrency = GetMemberWinlossInSystemCurrency(reader);

                    reader.Close();

                    GetOutputParams(criteria, objectDb);
                }
            }

            return playerWinlossReport;
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
        private IEnumerable<PlayerWinloss> GetMemberWinlossList(IDataReader reader)
        {
            var result = new List<PlayerWinloss>();

            while (reader.Read())
            {
                var playWinloss = new PlayerWinloss
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

                GetMemberWinlossColunmsInMemberCurrency(reader, playWinloss);
                GetMemberWinlossColunmsInSystemCurrency(reader, playWinloss);
                GetMemberWinlossInAffiliateCurrency(reader, playWinloss);

                result.Add(playWinloss);
            }

            return result;
        }

        /// <summary>
        /// Get member win-loss in member currencies.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Member win-loss list.</returns>
        private IEnumerable<PlayerWinloss> GetMemberWinlossInMemberCurrencies(IDataReader reader)
        {
            var result = new List<PlayerWinloss>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var playerWinloss = new PlayerWinloss
                    {
                        PlayerCurrency = reader.ToString("PlayerCurrency")
                    };

                    GetMemberWinlossColunmsInMemberCurrency(reader, playerWinloss);
                    GetMemberWinlossColunmsInSystemCurrency(reader, playerWinloss);
                    GetMemberWinlossInAffiliateCurrency(reader, playerWinloss);

                    result.Add(playerWinloss);
                }
            }

            return result;
        }

        /// <summary>
        /// Get member win-loss in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Member win-loss object.</returns>
        private PlayerWinloss GetMemberWinlossInSystemCurrency(IDataReader reader)
        {
            var playerWinloss = new PlayerWinloss();

            if (reader.NextResult())
            {
                reader.Read();
                GetMemberWinlossColunmsInSystemCurrency(reader, playerWinloss);
            }

            return playerWinloss;
        }

        /// <summary>
        /// Get member Win-loss columns which have values in member currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="playerWinloss">Player win-loss.</param>
        private void GetMemberWinlossColunmsInMemberCurrency(IDataReader reader, PlayerWinloss playerWinloss)
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
        private void GetMemberWinlossColunmsInSystemCurrency(IDataReader reader, PlayerWinloss playerWinloss)
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
        private void GetMemberWinlossInAffiliateCurrency(IDataReader reader, PlayerWinloss playerWinloss)
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
    }
}
