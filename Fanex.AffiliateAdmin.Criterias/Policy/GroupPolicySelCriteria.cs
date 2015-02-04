namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Policy Select Criteria.
    /// </summary>
    public class GroupPolicySelCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_GroupPolicySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}