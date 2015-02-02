namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Note Insert Criteria.
    /// </summary>
    public class NoteInsCriteria : CriteriaBase
    {
        public string Content { get; set; }

        public int AffId { get; set; }

        public int UpdatedBy { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_NoteIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}