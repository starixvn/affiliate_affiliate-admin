namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// IMediaService Interface.
    /// </summary>
    public interface IMediaService
    {
        IEnumerable<Banner> GetBanners(BannerListCriteria criteria);

        IEnumerable<Banner> GetTexts(BannerListCriteria criteria);

        BannerDetail GetBannerDetail(BannerDetailCriteria criteria);

        void AddBanner(BannerInsCriteria criteria);

        void EditBanner(BannerUpdateCriteria criteria);

        void DeleteBanner(BannerDeleteCriteria criteria);

        BannerType GetBannerTypeDetailDetail(BannerTypeDetailCriteria criteria);
    }
}