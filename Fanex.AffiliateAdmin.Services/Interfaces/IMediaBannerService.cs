namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;
    using System.Collections.Generic;

    public interface IMediaBannerService
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