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

        public DateTime WinlostDate { get; set; }

        public string Amount { get; set; }

        public string CsdEmail { get; set; }

        public string MailContact { get; set; }

        public string MemberSiteDomain { get; set; }

        public string MemberSiteUrl { get; set; }

        public string AdminSiteUrl { get; set; }

        public string AffiliateSiteUrl { get; set; }
    }
}