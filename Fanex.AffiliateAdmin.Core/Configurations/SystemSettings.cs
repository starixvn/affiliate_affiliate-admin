using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Core.Session;
using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.Criterias.Configuration.Product;
using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.AffiliateAdmin.Core.Configurations
{
    public class SystemSettings : ISystemSettings
    {
        private readonly IAppSetting _appSetting;
        private readonly ISystemSettingService _systemSettingService;
        public SystemSettings(
            IAppSetting appSetting,
            ISystemSettingService systemSettingService)
        {
            _appSetting = appSetting;
            _systemSettingService = systemSettingService;
        }
        public IEnumerable<Exchange> AffiliateCurrencies => _systemSettingService.GetExchangeList(new AffiliateCurrenciesSelCriteria());

        public IEnumerable<Language> AffLanguages => _systemSettingService.GetAffLanguageList(new AffLanguageCriteria());

        public IEnumerable<Language> AffSupportLanguages => _systemSettingService.GetAffSupportLanguageList(new AffiliateLanguageCriteria());

        public IEnumerable<Currency> AffSystemAllSupportedCurrencies => _systemSettingService.GetAffSystemAllSupportedCurrencyList(new AllSystemCurrencyCriteria());

        public IEnumerable<Currency> AffSystemSupportedCurrencies {
            get
            {
                var criteria = new SystemSupportedCurrencyCriteria { UserId = SessionManager.CurrentSession.CustID, LicenseeId = LicenseeID };

                return _systemSettingService.GetAffSystemSupportedCurrencyList(criteria);
            }
        }

        public IEnumerable<AffType> AffTypes
        {
            get
            {
                var affType = _systemSettingService.GetAffTypeList(new AffTypeCriteria());
                return affType.Select(t => new AffType()
                {
                    TypeID = t.TypeID,
                    Name = Lang.ResourceManager.GetString(t.Name)
                });
            }
        }

        public Exchange BaseExchange => _systemSettingService.GetSystemBaseExchange(new SystemSettingExchangeCriteria());

        public IEnumerable<Language> BaseLanguages => _systemSettingService.GetLanguageList(new AdminSupportedLanguagesSelCriteria());

        public IEnumerable<DeductionType> DeductionTypes => _systemSettingService.GetDeductionTypes(new DeductionTypeCriteria());

        public int LicenseeID
        {
            get
            {
                var licensee = _systemSettingService.GetSystemSetting(new SystemSettingsSelByNameCriteria
                {
                    Name = "LicenseeID",
                });

                return int.Parse(licensee.SValue);
            }
        }

        public IEnumerable<Language> MemberSiteSiteSupportedLanguages => _systemSettingService.GetMemberSiteSupportedLanguageList(
                new MemberSiteLanguageCriteria
                {
                    Environment = _appSetting.Environment
                });

        public IEnumerable<Currency> PlayerCurrencies => _systemSettingService.GetPlayerCurrencyList(new PlayerCurrencyCriteria());

        public IEnumerable<Product> Products => _systemSettingService.GetProductList(new ProductSelectCriteria());

        public string SystemName => GetSettingValue(new SystemSettingsSelByNameCriteria { Name = "SystemName" });

        public string SystemDisplayName
        {
            get
            {
                var displayName = GetSettingValue(new SystemSettingsSelByNameCriteria { Name = "SystemDisplayName" });

                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = SystemName;
                }

                return displayName;
            }
        }

        public DateTime SystemDateTime
        {
            get
            {
                return _systemSettingService.GetSystemDateTime();
            }
        }

        public DateTime SystemStartDate
        {
            get
            {
                var systemSetting = _systemSettingService.GetSystemSetting(new SystemSettingsSelByNameCriteria
                {
                    Name = "SystemStartDate"
                });

                return string.IsNullOrEmpty(systemSetting.SValue) ? SystemDateTime : Convert.ToDateTime(systemSetting.SValue);
            }
        }

        public IEnumerable<SystemSetting> PaymentMethods => _systemSettingService.GetPaymentMethod(new PaymentMethodCriteria());

        public IEnumerable<CommGroup> GetActiveCommGroups(CommGroupSelActiveListCriteria criteria)
        {
            return _systemSettingService.GetActiveCommGroups(criteria);
        }

        public IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria = null)
        {
            if (criteria == null)
            {
                criteria = new CommGroupCurrencyCriteria { Currencies = "All" };
            }

            var session = SessionManager.CurrentSession;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();

            return _systemSettingService.GetCommGroups(criteria);
        }

        public string GetSettingValue(SystemSettingsSelByNameCriteria detailCriteria)
        {
            string value = string.Empty;
            var systemSettings = _systemSettingService.GetSystemSettingList(detailCriteria);

            if (systemSettings != null && systemSettings.Any())
            {
                var selectedSystemSetting = systemSettings.FirstOrDefault(x => x.Environment.Equals(_appSetting.Environment, StringComparison.OrdinalIgnoreCase));

                if (selectedSystemSetting != null)
                {
                    value = selectedSystemSetting.SValue;
                }
            }

            return value;
        }

        public SelectList GetBaseCurrencyOptionList(bool isInMember, byte? selectedBaseCurrency = null)
        {
            var exchange = _systemSettingService.GetSystemBaseExchange(new SystemSettingExchangeCriteria());
            var baseCurrencyList = new List<SelectListItem>
            {
                new SelectListItem { Text = isInMember ? Lang.Member : Lang.Affiliate, Value = "0" },
                new SelectListItem { Text = exchange.Code, Value = "1" }
            };

            return new SelectList(baseCurrencyList, "Value", "Text", selectedBaseCurrency);
        }
    }
}
