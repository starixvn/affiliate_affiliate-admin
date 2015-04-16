// --------------------------------------------------------------------------
// <copyright file="MediaService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MediaService : QueryService, IMediaService
    {
        /// <summary>
        /// Get Banners.
        /// </summary>
        /// <param name="criteria">Banner Criteria.</param>
        /// <returns>Banner List.</returns>
        public IEnumerable<Banner> GetBanners(BannerListCriteria criteria)
        {
            var result = GetDataOutput<Banner>(criteria);

            foreach (var item in result)
            {
                var value = Lang.ResourceManager.GetString(item.Category.Replace(" ", string.Empty));

                item.Category = string.IsNullOrEmpty(value) ? item.Category : value;
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
                var value = Lang.ResourceManager.GetString(item.Category.Replace(" ", string.Empty));

                item.Category = string.IsNullOrEmpty(value) ? item.Category : value;
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