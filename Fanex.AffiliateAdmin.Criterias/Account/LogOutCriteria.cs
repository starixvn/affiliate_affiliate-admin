namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Logout Criteria.
    /// </summary>
    public class LogOutCriteria : CriteriaBase
    {
        public int OnUserId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Logout";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}