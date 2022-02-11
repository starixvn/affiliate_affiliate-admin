namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Commission group criteria class.
    /// </summary>
    public class AffiliateCommissionCriteria : CriteriaBase
    {
        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public string Currencies { get; set; }

        public byte BaseCurrency { get; set; }

        public byte SortBy { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte TransferStatus { get; set; }

        public byte GroupBy { get; set; }

        public int PageCurrent { get; set; }

        public int PageSize { get; set; }

        public int NRows { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateCommissionReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}