namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommGroupSelActiveListCriteria : CriteriaBase
    {
        public string Currencies { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroup_SelActiveList";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Currencies);
        }
    }
}
