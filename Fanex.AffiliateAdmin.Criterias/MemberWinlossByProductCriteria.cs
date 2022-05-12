namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberWinlossByProductCriteria : BaseCriteria
    {
        [SpParam(Ignored = true)]
        public bool ByAffiliate { get; set; }

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

        [SpParam(SqlParamName = "PageCurrent")]
        public new int PageIndex { get; set; }

        public int Duration { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_MemberWinLossByProductReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}