namespace Fanex.AffiliateAdmin.Criterias.Configuration.Country
{
    /// <summary>
    /// CountryAdd Criteria class.
    /// </summary>
    public class CountryInsertCriteria : CountryInsertUpdateCriteria
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_CountryIns";
        }
    }
}