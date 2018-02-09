namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Note Update Criteria.
    /// </summary>
    public class NoteUpdCriteria : CriteriaBase
    {
        public int Noteid { get; set; }

        public string Content { get; set; }

        public int UpdatedBy { get; set; }

        public int AffId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string AdminUserName { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_NoteUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}