namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// AffiliateList Criteria class.
    /// </summary>
    public class AffiliateListCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string Currencies { get; set; }

        public string CommGroups { get; set; }

        public byte DateOption { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte BaseCurrency { get; set; }

        public string PreferLanguages { get; set; }

        public byte SortBy { get; set; }

        public bool ExcludeTest { get; set; }

        [SpParam(SqlParamName = "FavoriteOnly")]
        public bool IsFavorite { get; set; }

        public string Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public decimal TotalAmount { get; set; }

        public int LicenseeId { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
