namespace Fanex.AffiliateAdmin.Criterias
{
    /// <summary>
    /// Group Policy Update Criteria.
    /// </summary>
    public class GroupPolicyUpdateCriteria : SimpleCriteriaBase
    {
        public string GroupPolicys { get; set; }

        public int UserID { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_GroupPolicyUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}