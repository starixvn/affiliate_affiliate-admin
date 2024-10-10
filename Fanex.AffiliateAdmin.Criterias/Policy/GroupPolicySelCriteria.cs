namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class GroupPolicySelCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_Con_GroupPolicySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}