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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MediaService : QueryService, IMediaService
    {
        public IEnumerable<Banner> GetBanners(BannerListCriteria criteria)
        {
            return GetDataOutput<Banner>(criteria);
        }

        public IEnumerable<Banner> GetTexts(BannerListCriteria criteria)
        {
            criteria.AffType = 0;
            return GetDataOutput<Banner>(criteria);
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