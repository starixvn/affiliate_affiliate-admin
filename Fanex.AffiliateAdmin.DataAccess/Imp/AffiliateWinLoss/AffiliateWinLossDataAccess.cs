// --------------------------------------------------------------------------
// <copyright file="AffiliateWinLossDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.17">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Extensions;

    /// <summary>
    /// Affiliate Win Loss Data Access.
    /// </summary>
    public abstract class AffiliateWinLossDataAccess : ReportDataAccess
    {
        protected virtual void GetColumnExtra(IDataReader reader, AffiliateWinLoss affiliateWinLoss)
        {
        }

        protected virtual void GetColumnExtra(IDataReader reader, AffiliateWinLoss affiliateWinLoss, string colSuffix)
        {
        }

        #region Helpers for commission

        /// <summary>
        /// Get simple commission list.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Commission list.</returns>
        protected IEnumerable<AffiliateWinLoss> GetCommissionList(IDataReader reader)
        {
            var result = new List<AffiliateWinLoss>();

            while (reader.Read())
            {
                var commission = new AffiliateWinLoss
                {
                    AffId = reader.ToInt("AffId"),
                    Status = reader.ToString("STATUS"),
                    Username = reader.ToString("Username"),
                    AffType = reader.ToString("AffType"),
                    Language = reader.ToString("LANGUAGE"),
                    CurrencyId = reader.ToInt("CurrencyId"),
                    Currency = reader.ToString("Currency"),
                    BetCount = reader.ToInt("BetCount"),
                    ActivePlayers = reader.ToInt("ActivePlayers"),
                    WinlostDate = reader.ToDateTime("WinlostDate"),
                    GroupID = reader.ToInt("GroupID"),
                    GroupName = reader.ToString("GroupName")
                };

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
        protected IEnumerable<AffiliateWinLoss> GetTotalCommissionInAffiliateCurrency(IDataReader reader)
        {
            var result = new List<AffiliateWinLoss>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var commission = new AffiliateWinLoss
                        {
                            CurrencyId = reader.ToInt("CurrencyId"),
                            Currency = reader.ToString("Currency"),
                            BetCount = reader.ToInt("BetCount"),
                            ActivePlayers = reader.ToInt("ActivePlayers")
                        };

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
        protected IEnumerable<AffiliateWinLoss> GetTotalCommissionInSystemCurrency(IDataReader reader)
        {
            var result = new List<AffiliateWinLoss>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var commission = new AffiliateWinLoss
                        {
                            BetCount = reader.ToInt("BetCount"),
                            ActivePlayers = reader.ToInt("ActivePlayers")
                        };

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
        private void GetCommissionColumns(IDataReader reader, AffiliateWinLoss commission, string colSuffix)
        {
            SetPropertyValue(reader, commission, string.Format("Turnover{0}", colSuffix));
            SetPropertyValue(reader, commission, string.Format("PlayerComm{0}", colSuffix));
            SetPropertyValue(reader, commission, string.Format("Winloss{0}", colSuffix));
            SetPropertyValue(reader, commission, string.Format("PShare{0}", colSuffix));
            SetPropertyValue(reader, commission, string.Format("PWin{0}", colSuffix));
            SetPropertyValue(reader, commission, string.Format("NetGaming{0}", colSuffix));

            GetColumnExtra(reader, commission, colSuffix);
        }
       
        #endregion Helpers for commission
    }
}