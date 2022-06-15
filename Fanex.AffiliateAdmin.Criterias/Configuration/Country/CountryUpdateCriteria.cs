namespace Fanex.AffiliateAdmin.Criterias.Configuration.Country
{
    /// <summary>
    /// CountryUpdate Criteria class.
    /// </summary>
    public class CountryUpdateCriteria : CountryInsertUpdateCriteria
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_CountryUpd";
        }
    }
}