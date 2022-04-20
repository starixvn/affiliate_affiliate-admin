namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class MediaSettings
    {
        private static IMediaSettingService _mediaSettingService = new MediaSettingService();
        private static IEnumerable<MediaType> _bannerCategories = _mediaSettingService.GetBannerCategories();

        public static IEnumerable<MediaType> BannerCategories
        {
            get
            {
                return _bannerCategories;
            }
        }

        public static string BannerDomain
        {
            get
            {
                return AppSettings.GetValue("BannerDomain");
            }
        }

        public static IEnumerable<string> BannerFolders
        {
            get
            {
                var value = AppSettings.GetValue("BannerFolders");
                return string.IsNullOrWhiteSpace(value) ? new string[] { } : value.Split(';');
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
    }
}