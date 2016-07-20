namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Deduction Type Criteria.
    /// </summary>
    public class DeductionTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_DeductionTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}