namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;

    public class MediaBannerService : QueryService, IMediaBannerService
    {
        public IEnumerable<Banner> GetBanners(BannerListCriteria criteria)
        {
            var result = GetDataOutput<Banner>(criteria);

            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item.Category))
                {
                    var category = Lang.ResourceManager.GetString(item.Category.Replace(" ", string.Empty));
                    item.Category = string.IsNullOrEmpty(category) ? item.Category : category;
                }

                if (!string.IsNullOrEmpty(item.ImageType))
                {
                    var image = Lang.ResourceManager.GetString(item.ImageType.Replace(" ", string.Empty));
                    item.ImageType = string.IsNullOrEmpty(image) ? item.ImageType : image;
                }
            }

            return result;
        }

        /// <summary>
        /// Get Texts.
        /// </summary>
        /// <param name="criteria">Banner Criteria.</param>
        /// <returns>Banner List.</returns>
        public IEnumerable<Banner> GetTexts(BannerListCriteria criteria)
        {
            criteria.AffType = 0;
            var result = GetDataOutput<Banner>(criteria);

            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item.Category))
                {
                    var category = Lang.ResourceManager.GetString(item.Category.Replace(" ", string.Empty));
                    item.Category = string.IsNullOrEmpty(category) ? item.Category : category;
                }
            }

            return result;
        }

        public BannerDetail GetBannerDetail(BannerDetailCriteria criteria)
        {
            return GetData<BannerDetail>(criteria).FirstOrDefault();
        }

        public void AddBanner(BannerInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void EditBanner(BannerUpdateCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void DeleteBanner(BannerDeleteCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public BannerType GetBannerTypeDetailDetail(BannerTypeDetailCriteria criteria)
        {
            return GetEntity<BannerType>(criteria);
        }
    }
}