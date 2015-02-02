namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Note Select Criteria.
    /// </summary>
    public class NoteSelCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int NoteId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_NoteSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}