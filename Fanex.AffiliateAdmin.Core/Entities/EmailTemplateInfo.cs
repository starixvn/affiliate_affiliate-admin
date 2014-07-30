// --------------------------------------------------------------------------
// <copyright file="EmailTemplateInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.22">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Entities
{
    using System;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Mailinator;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmailTemplateInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateInfo"/> class.
        /// </summary>
        public EmailTemplateInfo()
        {
            SystemName = AppSettings.SystemName;
            CsdEmail = AppSettings.MailContact;
            MailContact = AppSettings.MailContact;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateInfo"/> class.
        /// </summary>
        /// <param name="info">Site Domain Info.</param>
        public EmailTemplateInfo(SiteDomainInfo info)
            : this()
        {
            MemberSiteDomain = info.MemberSiteDomain;
            MemberSiteUrl = info.MemberSiteUrl;
            AdminSiteUrl = info.AdminSiteUrl;
            AffiliateSiteUrl = info.AffiliateSiteUrl;
        }

        public string SystemName { get; set; }

        public string Status { get; set; }

        public long TransId { get; set; }

        public string FirstName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime WinlostDate { get; set; }

        public string Amount { get; set; }

        public string CsdEmail { get; set; }

        public string MailContact { get; set; }

        public string MemberSiteDomain { get; set; }

        public Uri MemberSiteUrl { get; set; }

        public Uri AdminSiteUrl { get; set; }

        public Uri AffiliateSiteUrl { get; set; }

        public string HeaderBGColor
        {
            get
            {
                return EmailColorPalette.EmailHeaderBGColor;
            }
        }

        public string HeaderFontColor
        {
            get
            {
                return EmailColorPalette.EmailHeaderFontColor;
            }
        }

        public string ContentBGColor
        {
            get
            {
                return EmailColorPalette.EmailContentBGColor;
            }
        }

        public string ContentFontColor
        {
            get
            {
                return EmailColorPalette.EmailContentFontColor;
            }
        }

        public string CopyrightBGColor
        {
            get
            {
                return EmailColorPalette.EmailCopyrightBGColor;
            }
        }

        public string CopyrightFontColor
        {
            get
            {
                return EmailColorPalette.EmailCopyrightFontColor;
            }
        }

        public string FooterBGColor
        {
            get
            {
                return EmailColorPalette.EmailFooterBGColor;
            }
        }

        public string FooterFontColor
        {
            get
            {
                return EmailColorPalette.EmailFooterFontColor;
            }
        }

         public int CopyrightYear
        {
            get
            {
                return DateTime.Now.Year;
            }
        }
    }
}