namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class QualifiedMembersReportCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public byte BaseCurrency { get; set; }

        [SpParam(Ignored = true)]
        public string Currency { get; set; }

        public DateTime MonthDate { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        [SpParam(Ignored = true)]
        public int GroupId { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_QualifiedMembersReport";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Products);
        }
    }
}
