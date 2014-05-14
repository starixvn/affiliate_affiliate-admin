// --------------------------------------------------------------------------
// <copyright file="PaymentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string AffType { get; set; }

        public string Status { get; set; }

        public string PreferredLanguage { get; set; }

        public string Currency { get; set; }

        public short DateOption { get; set; }

        public DateTime? Fromdate { get; set; }

        public DateTime? Todate { get; set; }

        public byte BaseCurrency { get; set; }

        public bool IsITAdmin { get; set; }

        public bool ExcludeTest { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PaymentRequestSelList";
        }

        public override bool IsValid()
        {
            if (DateOption > 0 && Fromdate == null && Todate == null)
            {
                return false;
            }

            return true;
        }
    }
}