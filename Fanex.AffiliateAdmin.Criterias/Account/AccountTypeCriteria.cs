namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Account Type Criteria.
    /// </summary>
    public class AccountTypeCriteria : CriteriaBase
    {
        public bool IsIT { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AccountTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}