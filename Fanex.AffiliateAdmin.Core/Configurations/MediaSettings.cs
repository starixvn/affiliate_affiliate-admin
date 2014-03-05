// --------------------------------------------------------------------------
// <copyright file="MediaSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class MediaSettings
    {
        private static IMediaSettingService _mediaSettingService = new MediaSettingService();

        private static IEnumerable<MediaType> _bannerCategories;
        private static IEnumerable<MediaType> _bannerTypes;
        private static IEnumerable<MediaType> _bannerSizes;

        static MediaSettings()
        {
            _bannerCategories = _mediaSettingService.GetBannerCategories();
            _bannerTypes = _mediaSettingService.GetBannerTypes();
            _bannerSizes = _mediaSettingService.GetBannerSizes();
        }

        public static IEnumerable<MediaType> BannerCategories
        {
            get
            {
                return _bannerCategories;
            }
        }

        public static IEnumerable<MediaType> BannerTypes
        {
            get
            {
                return _bannerTypes;
            }
        }

        public static IEnumerable<MediaType> BannerSizes
        {
            get
            {
                return _bannerSizes;
            }
        }

        public static string BannerDomain
        {
            get
            {
                return AppSettings.GetSettingValue(new SystemSettingDetailCriteria
                {
                    Name = "BannerDomain",
                    Type = "Meta",
                    SearchBy = SearchByType.ByName
                });
            }
        }

        public static string BannerScriptUrl
        {
            get
            {
                return AppSettings.GetSettingValue(new SystemSettingDetailCriteria
                {
                    Name = "BannerScriptUrl",
                    Type = "Meta",
                    SearchBy = SearchByType.ByName
                });
            }
        }

        public static string BannerDirectory
        {
            get
            {
                return AppSettings.GetSettingValue(new SystemSettingDetailCriteria
                {
                    Name = "BannerDirectory",
                    Type = "Meta",
                    SearchBy = SearchByType.ByName
                });
            }
        }

        public static long MaxImageSize
        {
            get
            {
                return Convert.ToInt64(AppSettings.GetValue("MaxImageSize"));
            }
        }

        public static int ThumbnailHeight
        {
            get
            {
                return Convert.ToInt32(AppSettings.GetValue("ThumbnailHeight"));
            }
        }

        public static int ThumbnailWidth
        {
            get
            {
                return Convert.ToInt32(AppSettings.GetValue("ThumbnailWidth"));
            }
        }

        public static void RefreshBannerSizes()
        {
            _bannerSizes = _mediaSettingService.GetBannerSizes();
        }
    }
}