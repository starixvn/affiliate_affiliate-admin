namespace Fanex.AffiliateAdmin.Criterias.Configuration.Country
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CountryDetail Criteria class.
    /// </summary>
    public class CountryDetailCriteria : CriteriaBase
    {
        public string ID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CountrySelByID";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ID);
        }
    }
}