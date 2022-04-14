namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int SearchBy { get; set; }

        [SpParam(Ignored = true)]
        public bool DateChecked { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool? Important { get; set; }

        public string Language { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_HotNewsSel";
        }

        public override bool IsValid()
        {
            return !DateChecked || (FromDate != null && ToDate != null);
        }
    }
}