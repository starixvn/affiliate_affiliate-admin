namespace Fanex.AffiliateAdmin.Criterias.Configuration.Country
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CountrySelect Criteria class.
    /// </summary>
    public class CountrySelectCriteria : BaseCriteria
    {
        public string Name { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CountrySelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}