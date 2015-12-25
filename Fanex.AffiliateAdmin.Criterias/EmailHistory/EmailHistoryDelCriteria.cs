namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Email History Delete Criteria.
    /// </summary>
    public class EmailHistoryDelCriteria : CriteriaBase
    {
        public string Listmid { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_EmailHistoryDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}