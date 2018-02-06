namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommGroupCurrencyCriteria : CriteriaBase
    {
        public string Currencies { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupSelListNormal";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
