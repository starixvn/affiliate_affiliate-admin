namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Criteria class.
    /// </summary>
    public class CommGroupInfoCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public int AffID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupSelDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
