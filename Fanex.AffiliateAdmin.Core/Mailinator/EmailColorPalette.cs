// --------------------------------------------------------------------------
// <copyright file="EmailColorPalette.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.6">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Mailinator
{
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmailColorPalette
    {
        public static string EmailHeaderBgColor
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

        public static string EmailContentBgColor
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

        public static string EmailCopyrightBgColor
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

        public static string EmailFooterBgColor
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
