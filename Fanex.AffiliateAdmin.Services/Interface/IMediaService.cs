// --------------------------------------------------------------------------
// <copyright file="IMediaService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Interface
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