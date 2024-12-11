// -----------------------------------------------------------------------
// <copyright file="StatementCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StatementCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public string TransType { get; set; }

        public string Status { get; set; }

        public short DateOption { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte BaseCurrency { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_StatementHistorySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
