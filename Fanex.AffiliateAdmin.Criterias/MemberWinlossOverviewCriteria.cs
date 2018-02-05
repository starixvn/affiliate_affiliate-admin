namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Member Win Loss Overview Criteria.
    /// </summary>
    public class MemberWinLossOverviewCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public bool ByAffiliate { get; set; }

        public int UserId { get; set; }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public string PlayerName { get; set; }

        public byte GroupBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string PlayerCurrencies { get; set; }

        public byte BaseCurrency { get; set; }

        public byte DrawFilter { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        public int PageSize { get; set; }

        [SpParam(SqlParamName = "PageCurrent")]
        public int PageIndex { get; set; }

        public int NRows { get; set; }

        public int Duration { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public int LicenseeId { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_MemberWinLossReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}