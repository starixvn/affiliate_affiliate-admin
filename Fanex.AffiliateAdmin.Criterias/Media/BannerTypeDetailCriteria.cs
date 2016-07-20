namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Banner type detail.
    /// </summary>
    public class BannerTypeDetailCriteria : CriteriaBase
    {
        public int TypeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerTypeSel";
        }

        public override bool IsValid()
        {
            return TypeId > 0;
        }
    }
}