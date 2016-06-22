namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffLanguageCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}