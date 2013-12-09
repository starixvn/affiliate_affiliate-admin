// --------------------------------------------------------------------------
// <copyright file="MediaSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MediaSettingService : QueryService, IMediaSettingService
    {
        public IEnumerable<MediaType> GetBannerCategories()
        {
            return this.GetData<MediaType>(new BannerTypeCriteria
            {
                TypeName = "MediaCategory"
            });
        }

        public IEnumerable<MediaType> GetBannerTypes()
        {
            return this.GetData<MediaType>(new BannerTypeCriteria
            {
                TypeName = "ImageType"
            });
        }

        public IEnumerable<MediaType> GetBannerSizes()
        {
            return this.GetData<MediaType>(new BannerTypeCriteria
            {
                TypeName = "Size"
            });
        }

        public IEnumerable<MediaType> GetBannerSubCategories(short category)
        {
            return this.GetData<MediaType>(new BannerTypeCriteria
            {
                TypeName = "MediaSubCategory",
                ParentType = category
            });
        }
    }
}