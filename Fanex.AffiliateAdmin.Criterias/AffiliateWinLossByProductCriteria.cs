namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateWinLossByProductCriteria : CriteriaBase
    {
        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public string Currencies { get; set; }

        public byte GroupBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte DrawFilter { get; set; }

        public byte BaseCurrency { get; set; }

        public byte SortBy { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        public int PageSize { get; set; }

        [SpParam(SqlParamName = "PageCurrent")]
        public int PageIndex { get; set; }

        public int NRows { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateWinLossByProductReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
