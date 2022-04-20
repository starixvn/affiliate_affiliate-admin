namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Product;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class AppSetting : IAppSetting
    {
        #region Define variable

        private readonly Lazy<ISystemSettingService> _systemSetting;
        private readonly Lazy<IEnumerable<Exchange>> _affiliateCurrencies;
        private readonly Lazy<IEnumerable<Language>> _affLanguage;
        private readonly Lazy<IEnumerable<Language>> _affSupportLanguage;
        private readonly Lazy<IEnumerable<Currency>> _affSystemAllSupportedCurrencies;
        private readonly Lazy<IEnumerable<Currency>> _affSystemSupportedCurrencies;
        private readonly Lazy<IEnumerable<AffType>> _affType;
        private readonly Lazy<Exchange> _baseExchange;
        private readonly Lazy<IEnumerable<Language>> _baseLanguage;
        private readonly Lazy<IEnumerable<DeductionType>> _deductionTypes;

        private readonly Lazy<int> _licenseeId;

        private readonly Lazy<IEnumerable<Language>> _memberSiteSupportedLanguage;

        private readonly Lazy<IEnumerable<Currency>> _playerCurrencies;
        private readonly Lazy<IEnumerable<Product>> _products;
        private readonly Lazy<DateTime> _systemStartDate;

        private readonly Lazy<string> _systemName;
        private readonly Lazy<string> _systemDisplayName;

        #endregion Define variable

        public AppSetting()
        {
            _systemSetting = new Lazy<ISystemSettingService>(() => new SystemSettingService());
            _affiliateCurrencies = new Lazy<IEnumerable<Exchange>>(() => SystemSetting.GetExchangeList(new AffiliateCurrenciesSelCriteria()));
            _affLanguage = new Lazy<IEnumerable<Language>>(() => SystemSetting.GetAffLanguageList(new AffLanguageCriteria()));
            _affSupportLanguage = new Lazy<IEnumerable<Language>>(() => SystemSetting.GetAffSupportLanguageList(new AffiliateLanguageCriteria()));
            _affSystemAllSupportedCurrencies = new Lazy<IEnumerable<Currency>>(() => SystemSetting.GetAffSystemAllSupportedCurrencyList(new AllSystemCurrencyCriteria()));
            _affSystemSupportedCurrencies = new Lazy<IEnumerable<Currency>>(() =>
            {
                var criteria = new SystemSupportedCurrencyCriteria { UserId = SessionManager.CurrentSession.CustID, LicenseeId = LicenseeID };

                return SystemSetting.GetAffSystemSupportedCurrencyList(criteria);
            });
            _affType = new Lazy<IEnumerable<AffType>>(() =>
            {
                var affType = SystemSetting.GetAffTypeList(new AffTypeCriteria());
                return affType.Select(t => new AffType()
                {
                    TypeID = t.TypeID,
                    Name = Lang.ResourceManager.GetString(t.Name)
                });
            });
            _baseExchange = new Lazy<Exchange>(() => SystemSetting.GetSystemBaseExchange(new SystemSettingExchangeCriteria()));
            _baseLanguage = new Lazy<IEnumerable<Language>>(() => SystemSetting.GetLanguageList(new AdminSupportedLanguagesSelCriteria()));
            _deductionTypes = new Lazy<IEnumerable<DeductionType>>(() => SystemSetting.GetDeductionTypes(new DeductionTypeCriteria()));
            _licenseeId = new Lazy<int>(() =>
            {
                var licensee = SystemSetting.GetSystemSetting(new SystemSettingsSelByNameCriteria
                {
                    Name = "LicenseeID",
                });

                return int.Parse(licensee.SValue);
            });
            _memberSiteSupportedLanguage = new Lazy<IEnumerable<Language>>(() => SystemSetting.GetMemberSiteSupportedLanguageList(
                new MemberSiteLanguageCriteria
                {
                    Environment = Environment
                }));
            _playerCurrencies = new Lazy<IEnumerable<Currency>>(() => SystemSetting.GetPlayerCurrencyList(new PlayerCurrencyCriteria()));
            _products = new Lazy<IEnumerable<Product>>(() => SystemSetting.GetProductList(new ProductSelectCriteria()));
            _systemName = new Lazy<string>(() => GetSettingValue(new SystemSettingsSelByNameCriteria { Name = "SystemName" }));
            _systemDisplayName = new Lazy<string>(() =>
            {
                var displayName = GetSettingValue(new SystemSettingsSelByNameCriteria { Name = "SystemDisplayName" });

                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = SystemName;
                }

                return displayName;
            });
            _systemStartDate = new Lazy<DateTime>(() =>
            {
                var systemSetting = SystemSetting.GetSystemSetting(new SystemSettingsSelByNameCriteria
                {
                    Name = "SystemStartDate"
                });

                return string.IsNullOrEmpty(systemSetting.SValue)
                                            ? SystemTimeService.GetSystemDateTime()
                                            : Convert.ToDateTime(systemSetting.SValue);
            });
        }

        private ISystemSettingService SystemSetting
        {
            get
            {
                return _systemSetting.Value;
            }
        }

        public IEnumerable<Exchange> AffiliateCurrencies
        {
            get
            {
                return _affiliateCurrencies.Value;
            }
        }

        public IEnumerable<Language> AffLanguages
        {
            get
            {
                return _affLanguage.Value;
            }
        }

        public IEnumerable<Language> AffSupportLanguages
        {
            get
            {
                return _affSupportLanguage.Value;
            }
        }

        public IEnumerable<Currency> AffSystemAllSupportedCurrencies
        {
            get
            {
                return _affSystemAllSupportedCurrencies.Value;
            }
        }

        public IEnumerable<Currency> AffSystemSupportedCurrencies
        {
            get
            {
                return _affSystemSupportedCurrencies.Value;
            }
        }

        public IEnumerable<AffType> AffTypes
        {
            get
            {
                return _affType.Value;
            }
        }

        public Exchange BaseExchange
        {
            get
            {
                return _baseExchange.Value;
            }
        }

        public IEnumerable<Language> BaseLanguages
        {
            get
            {
                return _baseLanguage.Value;
            }
        }

        public IEnumerable<DeductionType> DeductionTypes
        {
            get
            {
                return _deductionTypes.Value;
            }
        }

        public string DefaultLanguage
        {
            get
            {
                return GetValue("DefaultLanguage");
            }
        }

        public string Environment
        {
            get
            {
                return GetValue("Environment");
            }
        }

        public Uri InternalUrl
        {
            get
            {
                return new Uri(GetValue("InternalURL"));
            }
        }

        public Uri InternalUrlKiosk
        {
            get
            {
                return new Uri(GetValue("InternalURL_Kiosk"));
            }
        }

        public Uri SiteCheckerUrl
        {
            get
            {
                return new Uri(GetValue("SiteCheckerURL"));
            }
        }

        public Uri IPCheckerUrl
        {
            get
            {
                return new Uri(GetValue("IPCheckerURL"));
            }
        }

        public string ITMail
        {
            get
            {
                return GetValue("ITMail");
            }
        }

        public string Layout
        {
            get
            {
                return GetValue("Layout");
            }
        }

        public int LeftString
        {
            get
            {
                return Convert.ToInt32(GetValue("LeftString"));
            }
        }

        public int LicenseeID
        {
            get
            {
                return _licenseeId.Value;
            }
        }

        public bool LocalTest
        {
            get
            {
                return Environment.Equals("Local Development", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool IsProduction
        {
            get
            {
                return Environment.Equals("Production");
            }
        }

        public string MailContact
        {
            get
            {
                return GetValue("MailContact");
            }
        }

        public IEnumerable<Language> MemberSiteSiteSupportedLanguages
        {
            get
            {
                return _memberSiteSupportedLanguage.Value;
            }
        }

        public decimal MinimumWithdrawalAmount
        {
            get
            {
                return Convert.ToDecimal(GetValue("MinimumWithdrawalAmount"));
            }
        }

        public int PageSize
        {
            get
            {
                return Convert.ToInt32(GetValue("PageSize"));
            }
        }

        public IEnumerable<Currency> PlayerCurrencies
        {
            get
            {
                return _playerCurrencies.Value;
            }
        }

        public IEnumerable<Product> Products
        {
            get
            {
                return _products.Value;
            }
        }

        public int ServerId
        {
            get
            {
                return Convert.ToInt32(GetValue("ServerID"));
            }
        }

        public string ServerTimeZone
        {
            get
            {
                var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                return string.Format("{0}{1:00}:00", offset.Hours > 0 ? "+" : string.Empty, offset.Hours);
            }
        }

        public string SystemName
        {
            get
            {
                return _systemName.Value;
            }
        }

        public string SystemDisplayName
        {
            get
            {
                return _systemDisplayName.Value;
            }
        }

        public DateTime SystemStartDate
        {
            get
            {
                return _systemStartDate.Value;
            }
        }

        public string Theme
        {
            get
            {
                return GetValue("Theme");
            }
        }

        public int ViewLogBackMonth
        {
            get
            {
                return Convert.ToInt32(GetValue("ViewLogBackMonth"));
            }
        }

        public string DefaultCurrency
        {
            get
            {
                return GetValue("DefaultCurrency");
            }
        }

        public IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria = null)
        {
            if (criteria == null)
            {
                criteria = new CommGroupCurrencyCriteria { Currencies = "All" };
            }

            var session = SessionManager.CurrentSession;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();

            return SystemSetting.GetCommGroups(criteria);
        }

        /// <summary>
        /// Get Setting value.
        /// </summary>
        /// <param name="detailCriteria">System Setting Detail Criteria.</param>
        /// <returns>Setting value.</returns>
        public string GetSettingValue(SystemSettingsSelByNameCriteria detailCriteria)
        {
            string value = string.Empty;
            var systemSettings = SystemSetting.GetSystemSettingList(detailCriteria);

            if (systemSettings != null && systemSettings.Any())
            {
                var selectedSystemSetting = systemSettings.FirstOrDefault(x => x.Environment.Equals(Environment, StringComparison.OrdinalIgnoreCase));

                if (selectedSystemSetting != null)
                {
                    value = selectedSystemSetting.SValue;
                }
            }

            return value;
        }

        /// <summary>
        /// Get key value of resource.
        /// </summary>
        /// <param name="key">Resource key.</param>
        /// <returns>Return value of resource key.</returns>
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
