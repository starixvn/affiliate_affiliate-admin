namespace Fanex.AffiliateAdmin.Core.Mailinator
{
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class EmailColorPalette
    {
        public static string EmailHeaderBGColor
        {
            get
            {
                return GetAppSettings<string>.GetValue("EmailHeaderBgColor");
            }
        }

        public static string EmailHeaderFontColor
        {
            get
            {
                return GetAppSettings<string>.GetValue("EmailHeaderFontColor");
            }
        }

        public static string EmailContentBGColor
        {
            get
            {
                return GetAppSettings<string>.GetValue("EmailContentBgColor");
            }
        }

        public static string EmailContentFontColor
        {
            get
            {
                return GetAppSettings<string>.GetValue("EmailContentFontColor");
            }
        }

        public static string EmailCopyrightBGColor
        {
            get
            {
                return GetAppSettings<string>.GetValue("EmailCopyrightBgColor");
            }
        }

        public static string EmailCopyrightFontColor
        {
            get
            {
                return GetAppSettings<string>.GetValue("EmailCopyrightFontColor");
            }
        }

        public static string EmailFooterBGColor
        {
            get
            {
                return GetAppSettings<string>.GetValue("EmailFooterBgColor");
            }
        }

        public static string EmailFooterFontColor
        {
            get
            {
                return GetAppSettings<string>.GetValue("EmailFooterFontColor");
            }
        }
    }
}
