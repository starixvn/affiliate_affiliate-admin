namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SystemDateTimeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateSelDBTime";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}