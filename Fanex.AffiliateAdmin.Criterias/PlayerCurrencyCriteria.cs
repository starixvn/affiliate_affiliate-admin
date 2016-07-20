namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerCurrencyCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_PlayersCurrenciesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}