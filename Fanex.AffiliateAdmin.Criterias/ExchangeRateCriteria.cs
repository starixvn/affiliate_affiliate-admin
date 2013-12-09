// --------------------------------------------------------------------------
// <copyright file="ExchangeRateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.29">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Exchange Rate Criteria.
    /// </summary>
    public class ExchangeRateCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public CriteriaType Type { get; set; }

        public int UserId { get; set; }

        public int ExchangeId { get; set; }

        public string ExchangeIdList { get; set; }

        public DateTime? Time { get; set; }

        public double Rate { get; set; }

        public string ToCurrency { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            if (this.Type == CriteriaType.Update)
            {
                if (!string.IsNullOrWhiteSpace(this.ToCurrency))
                {
                    return "AffAdmin_ExchangeUpdate";                       
                }
                else if (!string.IsNullOrWhiteSpace(this.ExchangeIdList))
                {
                    return "AffAdmin_CopyRateFromMarket";
                }
                else
                {
                    return "AffAdmin_ExchangeAutoRefreshUpd";
                }
            }
            else
            {
                return "AffAdmin_ExchangeSel";
            }
        }
    }
}