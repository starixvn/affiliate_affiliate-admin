// --------------------------------------------------------------------------
// <copyright file="MemberWinLossDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Extensions;

    /// <summary>
    /// Member Win Loss Data Access.
    /// </summary>
    public abstract class MemberWinLossDataAccess : ReportDataAccess
    {
        protected virtual void GetColumnExtra(IDataReader reader, MemberWinLossItem memberWinLoss)
        {
        }

        protected virtual void GetColumnExtra(IDataReader reader, MemberWinLossItem memberWinLoss, string colSuffix)
        {
        }

        protected virtual void GetColumnExtraInTotalResult(IDataReader reader, MemberWinLossItem memberWinLoss)
        {
        }

        #region Helpers for member win-loss

        /// <summary>
        /// Get member win-loss list.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Member win-loss list.</returns>
        protected IEnumerable<MemberWinLossItem> GetMemberWinlossList(IDataReader reader)
        {
            var result = new List<MemberWinLossItem>();

            while (reader.Read())
            {
                var memberWinloss = new MemberWinLossItem
                {
                    RowIdx = reader.ToLong("RowIDX"),
                    WinlostDate = reader.ToDateTime("WinlostDate"),
                    AffId = reader.ToInt("AffId"),
                    Username = reader.ToString("Username"),
                    Currency = reader.ToString("Currency"),
                    PlayerUsername = reader.ToString("PlayerUsername"),
                    PlayerCurrency = reader.ToString("PlayerCurrency"),
                    BetCount = reader.ToInt("BetCount"),
                    ExRate = reader.ToDouble("ExRate"),
                    AffExRate = reader.ToDouble("AffExRate"),
                    AffCurrencyId = reader.ToInt("AffCurrencyId"),
                    PlayerCurrencyId = reader.ToInt("PlayerCurrencyId")
                };

                GetColumnExtra(reader, memberWinloss);

                GetMemberWinlossColumnsBasedOnCurrency(reader, memberWinloss);

                result.Add(memberWinloss);
            }

            return result;
        }

        /// <summary>
        /// Get total member win-loss in member currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Total member win-loss list.</returns>
        protected IEnumerable<MemberWinLossItem> GetTotalMemberWinlossInMemberCurrency(IDataReader reader)
        {
            var result = new List<MemberWinLossItem>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var memberWinloss = new MemberWinLossItem
                    {
                        PlayerCurrency = reader.ToString("PlayerCurrency"),
                        BetCount = reader.ToInt("BetCount")
                    };

                    GetColumnExtraInTotalResult(reader, memberWinloss);

                    GetMemberWinlossColumnsBasedOnCurrency(reader, memberWinloss);

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
        protected IEnumerable<MemberWinLossItem> GetTotalMemberWinlossInSystemCurrency(IDataReader reader)
        {
            var result = new List<MemberWinLossItem>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    var memberWinloss = new MemberWinLossItem { BetCount = reader.ToInt("BetCount") };

                    GetColumnExtraInTotalResult(reader, memberWinloss);

                    GetMemberWinLossColumns(reader, memberWinloss, SysColSuffix);

                    result.Add(memberWinloss);
                }
            }

            return result;
        }

        /// <summary>
        /// Get member win-loss columns in system currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        /// <param name="colSuffix">Column suffix.</param>
        private void GetMemberWinLossColumns(IDataReader reader, MemberWinLossItem memberWinloss, string colSuffix)
        {
            SetPropertyValue(reader, memberWinloss, string.Format("TotalTurnover{0}", colSuffix));
            SetPropertyValue(reader, memberWinloss, string.Format("PlayerComm{0}", colSuffix));
            SetPropertyValue(reader, memberWinloss, string.Format("MemberWinLoss{0}", colSuffix));

            SetPropertyValue(reader, memberWinloss, string.Format("PShare{0}", colSuffix));
            SetPropertyValue(reader, memberWinloss, string.Format("PWin{0}", colSuffix));
            SetPropertyValue(reader, memberWinloss, string.Format("NetGaming{0}", colSuffix));

            GetColumnExtra(reader, memberWinloss, colSuffix);
        }

        /// <summary>
        /// Get member win-loss columns based on base currency.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="memberWinloss">Member win-loss.</param>
        private void GetMemberWinlossColumnsBasedOnCurrency(IDataReader reader, MemberWinLossItem memberWinloss)
        {
            GetMemberWinLossColumns(reader, memberWinloss, SysColSuffix);

            GetMemberWinLossColumns(reader, memberWinloss, string.Empty);

            GetMemberWinLossColumns(reader, memberWinloss, AffColSuffix);
        }

        #endregion Helpers for member win-loss
    }
}