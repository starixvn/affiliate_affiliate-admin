namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerDetailCriteria : CriteriaBase
    {
        public int BannerId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerSelDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}