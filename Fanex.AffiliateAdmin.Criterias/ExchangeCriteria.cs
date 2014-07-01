// --------------------------------------------------------------------------
// <copyright file="ExchangeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Exchange Criteria.
    /// </summary>
    public class ExchangeCriteria : CriteriaBase
    {
        public ExchangeCriteria()
        {
            this.Type = "Affiliate";
        }

        [SpParam(Ignored = true)]
        public string Type { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            string settingKey = string.Empty;
            switch (Type)
            {
                case "Affiliate":
                    settingKey = "AffAdmin_AffiliateCurrenciesSel";
                    break;
                case "Base":
                    settingKey = "AffAdmin_BaseCurrenciesSel";
                    break;
                case "System":
                    settingKey = "AffAdmin_CurrencyListSel";
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}