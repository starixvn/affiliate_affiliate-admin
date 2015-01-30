namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Email History Select Criteria.
    /// </summary>
    public class EmailHistorySelCriteria : CriteriaBase
    {
        public int? MID { get; set; }

        public string SenderName { get; set; }

        public string Email { get; set; }

        public string AffiliateUserName { get; set; }

        public DateTime Fromdate { get; set; }

        public DateTime Todate { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_EmailHistorySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}