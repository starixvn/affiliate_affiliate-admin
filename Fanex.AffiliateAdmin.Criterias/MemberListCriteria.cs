namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class MemberListCriteria : PagingCriteriaBase
    {
        public string AffUsername { get; set; }

        public string Username { get; set; }

        public string AffType { get; set; }

        public string CurrencyList { get; set; }

        public byte MemberType { get; set; }

        public byte DateOption { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public byte SortBy { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        public string UserCurrencies { get; set; }

        [SpParam(Ignored = true)]
        public bool ByAffiliate { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_MemberListSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}