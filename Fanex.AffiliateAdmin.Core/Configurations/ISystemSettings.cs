using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
using Fanex.AffiliateAdmin.DTO;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.AffiliateAdmin.Core.Configurations
{
    public interface ISystemSettings
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

        int LicenseeID { get; }

        IEnumerable<Language> MemberSiteSiteSupportedLanguages { get; }

        IEnumerable<Currency> PlayerCurrencies { get; }

        IEnumerable<Product> Products { get; }

        string SystemName { get; }

        string SystemDisplayName { get; }

        DateTime SystemDateTime { get; }

        DateTime SystemStartDate { get; }

        IEnumerable<SystemSetting> PaymentMethods { get; }

        IEnumerable<CommGroup> GetActiveCommGroups(CommGroupSelActiveListCriteria criteria);

        IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria = null);

        string GetSettingValue(SystemSettingsSelByNameCriteria detailCriteria);

        SelectList GetBaseCurrencyOptionList(bool isInMember, byte? selectedBaseCurrency = null);
    }
}
