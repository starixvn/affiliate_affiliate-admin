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
        IEnumerable<Exchange> AffiliateCurrencies { get; }

        IEnumerable<Language> AffLanguages { get; }

        IEnumerable<Language> AffSupportLanguages { get; }

        IEnumerable<Currency> AffSystemAllSupportedCurrencies { get; }

        IEnumerable<Currency> AffSystemSupportedCurrencies { get; }

        IEnumerable<AffType> AffTypes { get; }

        Exchange BaseExchange { get; }

        IEnumerable<Language> BaseLanguages { get; }

        IEnumerable<DeductionType> DeductionTypes { get; }

        string DefaultLanguage { get; }

        string Environment { get; }

        Uri InternalUrl { get; }

        Uri InternalUrlKiosk { get; }

        Uri SiteCheckerUrl { get; }

        Uri IPCheckerUrl { get; }

        string ITMail { get; }

        string Layout { get; }

        int LeftString { get; }

        int LicenseeID { get; }

        bool LocalTest { get; }

        bool IsProduction { get; }

        string MailContact { get; }

        IEnumerable<Language> MemberSiteSiteSupportedLanguages { get; }

        decimal MinimumWithdrawalAmount { get; }

        int PageSize { get; }

        IEnumerable<Currency> PlayerCurrencies { get; }

        IEnumerable<Product> Products { get; }

        int ServerId { get; }

        string ServerTimeZone { get; }

        string SystemName { get; }

        string SystemDisplayName { get; }

        DateTime SystemStartDate { get; }

        string Theme { get; }

        int ViewLogBackMonth { get; }

        string DefaultCurrency { get; }

        IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria = null);

        string GetSettingValue(SystemSettingsSelByNameCriteria detailCriteria);

        string GetValue(string key);
    }
}
