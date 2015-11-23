namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Note Delete Criteria.
    /// </summary>
    public class NoteDelCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public string ListNode { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_NoteDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}