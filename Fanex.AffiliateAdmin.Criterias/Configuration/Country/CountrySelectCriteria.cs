namespace Fanex.AffiliateAdmin.Criterias.Configuration.Country
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CountrySelect Criteria class.
    /// </summary>
    public class CountrySelectCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

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