namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.DTO;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    public interface IAppSetting
    {
        string DefaultLanguage { get; }

        string Environment { get; }

        Uri InternalUrl { get; }

        Uri InternalUrlKiosk { get; }

        Uri SiteCheckerUrl { get; }

        Uri IPCheckerUrl { get; }

        string ITMail { get; }

        string Layout { get; }

        int LeftString { get; }

        bool LocalTest { get; }

        bool IsProduction { get; }

        string MailContact { get; }

        decimal MinimumWithdrawalAmount { get; }

        int PageSize { get; }

        int ServerId { get; }

        string ServerTimeZone { get; }

        string Theme { get; }

        int ViewLogBackMonth { get; }

        string DefaultCurrency { get; }

        string BannerDomain { get; }

        IEnumerable<string> BannerFolders { get; }

        long MaxImageSize { get; }

        int ThumbnailHeight { get; }

        int ThumbnailWidth { get; }

        string GetValue(string key);
    }
}
