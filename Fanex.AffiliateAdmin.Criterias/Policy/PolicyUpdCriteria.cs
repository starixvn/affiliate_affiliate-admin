namespace Fanex.AffiliateAdmin.Criterias
{
    /// <summary>
    /// User Policy Update Criteria.
    /// </summary>
    public class UserPolicyUpdateCriteria : SimpleCriteriaBase
    {
        public string MemPolicys { get; set; }

        public int UserID { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_PolicyUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}