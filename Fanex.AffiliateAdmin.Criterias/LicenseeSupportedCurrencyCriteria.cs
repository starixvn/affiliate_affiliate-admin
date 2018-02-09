namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class LicenseeSupportedCurrencyCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_GetLicenseeSupportedCurrencies";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}