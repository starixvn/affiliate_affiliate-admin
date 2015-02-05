namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// User Policy Select Criteria.
    /// </summary>
    public class UserPolicySelCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public int GroupID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_PolicySelByCustID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}