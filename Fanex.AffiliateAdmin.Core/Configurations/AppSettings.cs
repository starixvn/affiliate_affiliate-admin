namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Criterias.Configuration.Product;
    using Criterias.Configuration.SystemSetting;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Services;
    using Services.Interface;
    using Session;

    /// <summary>
    /// AppSettings class.
    /// </summary>
    public static class AppSettings
    {
        #region Define variable

        private static Lazy<ISystemSettingService> _systemSetting;
        private static Lazy<IEnumerable<Exchange>> _affiliateCurrencies;
        private static Lazy<IEnumerable<Language>> _affLanguage;
        private static Lazy<IEnumerable<Language>> _affSupportLanguage;
        private static Lazy<IEnumerable<Currency>> _affSystemAllSupportedCurrencies;
        private static Lazy<IEnumerable<Currency>> _affSystemSupportedCurrencies;
        private static Lazy<IEnumerable<AffType>> _affType;
        private static Lazy<Exchange> _baseExchange;
        private static Lazy<IEnumerable<Language>> _baseLanguage;
        private static Lazy<IEnumerable<DeductionType>> _deductionTypes;

        private static Lazy<int> _licenseeId;

        private static Lazy<IEnumerable<Language>> _memberSiteSupportedLanguage;

        private static Lazy<IEnumerable<Currency>> _playerCurrencies;
        private static Lazy<IEnumerable<Product>> _products;
        private static Lazy<DateTime> _systemStartDate;

        private static Lazy<string> _systemName;
        private static Lazy<string> _systemDisplayName;

        #endregion Define variable

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        static AppSettings()
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
            _products = new Lazy<IEnumerable<Product>>(() => SystemSetting.GetProductList(new ProductSelectCriteria { LicenseeId = LicenseeID }));
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

        private static ISystemSettingService SystemSetting
        {
            get
            {
                return _systemSetting.Value;
            }
        }

        public static IEnumerable<Exchange> AffiliateCurrencies
        {
            get
            {
                return _affiliateCurrencies.Value;
            }
        }

        public static string AffiliateName
        {
            get { return SystemName; }
        }

        public static IEnumerable<Language> AffLanguages
        {
            get
            {
                return _affLanguage.Value;
            }
        }

        public static IEnumerable<Language> AffSupportLanguages
        {
            get
            {
                return _affSupportLanguage.Value;
            }
        }

        public static IEnumerable<Currency> AffSystemAllSupportedCurrencies
        {
            get
            {
                return _affSystemAllSupportedCurrencies.Value;
            }
        }

        public static IEnumerable<Currency> AffSystemSupportedCurrencies
        {
            get
            {
                return _affSystemSupportedCurrencies.Value;
            }
        }

        public static IEnumerable<AffType> AffTypes
        {
            get
            {
                return _affType.Value;
            }
        }

        public static List<SelectListItem> BaseCurrencyInAffiliate
        {
            get
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem
                        {
                            Text = Lang.Affiliate,
                            Value = "0"
                        },
                        new SelectListItem
                        {
                            Text = BaseExchange.Code,
                            Value = "1"
                        }
                    };
            }
        }

        public static List<SelectListItem> BaseCurrencyInMember
        {
            get
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem
                            {
                                Text = Lang.Member,
                                Value = "0"
                            },
                        new SelectListItem
                            {
                                Text = BaseExchange.Code,
                                Value = "1"
                            }
                    };
            }
        }

        public static Exchange BaseExchange
        {
            get
            {
                return _baseExchange.Value;
            }
        }

        public static string PiwikSiteId
        {
            get
            {
                return GetValue("PiwikSiteId");
            }
        }

        public static IEnumerable<Language> BaseLanguages
        {
            get
            {
                return _baseLanguage.Value;
            }
        }

        public static IEnumerable<DeductionType> DeductionTypes
        {
            get
            {
                return _deductionTypes.Value;
            }
        }

        public static string DefaultLanguage
        {
            get
            {
                return GetValue("DefaultLanguage");
            }
        }

        public static bool EnableTransformer
        {
            get
            {
                return int.Parse(GetValue("EnableTransformer")) == 1;
            }
        }

        public static string Environment
        {
            get
            {
                return GetValue("Environment");
            }
        }

        public static Uri InternalUrl
        {
            get
            {
                return new Uri(GetValue("InternalURL"));
            }
        }

        public static Uri InternalUrlKiosk
        {
            get
            {
                return new Uri(GetValue("InternalURL_Kiosk"));
            }
        }

        public static Uri SiteCheckerUrl
        {
            get
            {
                return new Uri(GetValue("SiteCheckerURL"));
            }
        }

        public static Uri IPCheckerUrl
        {
            get
            {
                return new Uri(GetValue("IPCheckerURL"));
            }
        }

        public static string ITMail
        {
            get
            {
                return GetValue("ITMail");
            }
        }

        public static string Layout
        {
            get
            {
                return GetValue("Layout");
            }
        }

        public static int LeftString
        {
            get
            {
                return Convert.ToInt32(GetValue("LeftString"));
            }
        }

        public static int LicenseeID
        {
            get
            {
                return _licenseeId.Value;
            }
        }

        public static bool LocalTest
        {
            get
            {
                return Environment.Equals("Local Development", StringComparison.OrdinalIgnoreCase);
            }
        }

        public static bool IsProduction
        {
            get
            {
                return Environment.Equals("Production");
            }
        }

        public static string MailContact
        {
            get
            {
                return GetValue("MailContact");
            }
        }

        public static IEnumerable<Language> MemberSiteSiteSupportedLanguages
        {
            get
            {
                return _memberSiteSupportedLanguage.Value;
            }
        }

        public static decimal MinimumWithdrawalAmount
        {
            get
            {
                return Convert.ToDecimal(GetValue("MinimumWithdrawalAmount"));
            }
        }

        public static int PageSize
        {
            get
            {
                return Convert.ToInt32(GetValue("PageSize"));
            }
        }

        public static IEnumerable<Currency> PlayerCurrencies
        {
            get
            {
                return _playerCurrencies.Value;
            }
        }

        public static IEnumerable<Product> Products
        {
            get
            {
                return _products.Value;
            }
        }

        public static int ServerId
        {
            get
            {
                return Convert.ToInt32(GetValue("ServerID"));
            }
        }

        public static string ServerTimeZone
        {
            get
            {
                var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                return string.Format("{0}{1:00}:00", offset.Hours > 0 ? "+" : string.Empty, offset.Hours);
            }
        }

        public static string SystemName
        {
            get
            {
                return _systemName.Value;
            }
        }

        public static string SystemDisplayName
        {
            get
            {
                return _systemDisplayName.Value;
            }
        }

        public static DateTime SystemStartDate
        {
            get
            {
                return _systemStartDate.Value;
            }
        }

        public static string Theme
        {
            get
            {
                return GetValue("Theme");
            }
        }

        public static int ViewLogBackMonth
        {
            get
            {
                return Convert.ToInt32(GetValue("ViewLogBackMonth"));
            }
        }

        public static IEnumerable<CommGroup> GetActiveCommGroups(CommGroupSelActiveListCriteria criteria)
        {
            criteria.LicenseeId = LicenseeID;
            return SystemSetting.GetActiveCommGroups(criteria);
        }

        public static IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria)
        {
            if (criteria == null)
            {
                criteria = new CommGroupCurrencyCriteria { Currency = "All" };
            }

            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.LicenseeId = LicenseeID;

            return SystemSetting.GetCommGroups(criteria);
        }

        /// <summary>
        /// Get Setting value.
        /// </summary>
        /// <param name="detailCriteria">System Setting Detail Criteria.</param>
        /// <returns>Setting value.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Accepted")]
        public static string GetSettingValue(SystemSettingsSelByNameCriteria detailCriteria)
        {
            string value = string.Empty;
            var systemSettings = SystemSetting.GetSystemSettingList(detailCriteria) as List<SystemSetting>;

            if (systemSettings != null && systemSettings.Count > 0)
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
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}