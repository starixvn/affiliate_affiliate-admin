namespace Fanex.AffiliateAdmin.Criterias.Configuration.Product
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ProductSelect Criteria class.
    /// </summary>
    public class ProductSelectCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ProductSelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}