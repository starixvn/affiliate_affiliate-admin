// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.29">Created</change>
//   <change who="Kaden.Trinh" date="2013.06.24">Add setting key for e-mail</change>
//   <change who="Kaden.Trinh" date="2013.06.25">Load all setting default from DB</change>
// </history>

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

        private static readonly ISystemSettingService SystemSetting = new SystemSettingService();
        private static IEnumerable<Exchange> _affiliateCurrencies = SystemSetting.GetExchangeList(new AffiliateCurrenciesSelCriteria());
        private static IEnumerable<Language> _affLanguage = SystemSetting.GetAffLanguageList(new AffLanguageCriteria());
        private static IEnumerable<Language> _affSupportLanguage = SystemSetting.GetAffSupportLanguageList(new AffiliateLanguageCriteria());
        private static IEnumerable<Currency> _affSystemAllSupportedCurrencies;
        private static IEnumerable<Currency> _affSystemSupportedCurrencies;
        private static IEnumerable<AffType> _affType = SystemSetting.GetAffTypeList(new AffTypeCriteria());
        private static Exchange _baseExchange = SystemSetting.GetSystemBaseExchange(new SystemSettingExchangeCriteria());
        private static IEnumerable<Language> _baseLanguage = SystemSetting.GetLanguageList(new AdminSupportedLanguagesSelCriteria());
        private static IEnumerable<DeductionType> _deductionTypes;        

        private static int? _licenseeId;

        private static IEnumerable<Language> _memberSiteSupportedLanguage = SystemSetting.GetMemberSiteSupportedLanguageList(new MemberSiteLanguageCriteria
            {
                Environment = Environment
            });

        private static IEnumerable<Currency> _playerCurrencies;
        private static IEnumerable<Product> _products;
        private static DateTime? _systemStartDate;

        #endregion Define variable

        public static IEnumerable<Exchange> AffiliateCurrencies
        {
            get
            {
                return _affiliateCurrencies;
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
                return _affLanguage;
            }
        }

        public static IEnumerable<Language> AffSupportLanguages
        {
            get
            {
                return _affSupportLanguage;
            }
        }

        public static IEnumerable<Currency> AffSystemAllSupportedCurrencies
        {
            get
            {
                if (_affSystemAllSupportedCurrencies == null)
                {
                    var criteria = new AllSystemCurrencyCriteria();
                    _affSystemAllSupportedCurrencies = SystemSetting.GetAffSystemAllSupportedCurrencyList(criteria);
                }

                return _affSystemAllSupportedCurrencies;
            }
        }

        public static IEnumerable<Currency> AffSystemSupportedCurrencies
        {
            get
            {
                if (_affSystemSupportedCurrencies == null)
                {
                    var criteria = new SystemSupportedCurrencyCriteria { UserId = SessionManager.CurrentSession.CustID };

                    _affSystemSupportedCurrencies = SystemSetting.GetAffSystemSupportedCurrencyList(criteria);
                }

                return _affSystemSupportedCurrencies;
            }
        }

        public static IEnumerable<AffType> AffTypes
        {
            get
            {
                 var translatedAffTypes = _affType.Select(t => new AffType()
                    {
                        TypeID = t.TypeID,
                        Name = Lang.ResourceManager.GetString(t.Name)
                    });

                return translatedAffTypes;
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
                return _baseExchange;
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
                return _baseLanguage;
            }
        }

        public static IEnumerable<DeductionType> DeductionTypes
        {
            get
            {
                return _deductionTypes ?? (_deductionTypes = SystemSetting.GetDeductionTypes(new DeductionTypeCriteria()));
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

        public static Uri LBUrl
        {
            get
            {
                return string.IsNullOrEmpty(GetValue("LBUrl")) ? null : new Uri(GetValue("LBUrl"));
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
                if (_licenseeId == null)
                {
                    var licensee = SystemSetting.GetSystemSetting(new SystemSettingsSelByNameCriteria
                    {
                        Name = "LicenseeID",
                    });

                    _licenseeId = int.Parse(licensee.SValue);
                }

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
                return _memberSiteSupportedLanguage;
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
                _playerCurrencies = _playerCurrencies ?? SystemSetting.GetPlayerCurrencyList(new PlayerCurrencyCriteria());

                return _playerCurrencies;
            }
        }

        public static IEnumerable<Product> Products
        {
            get
            {
                if (_products == null)
                {
                    _products = SystemSetting.GetProductList(new ProductSelectCriteria());
                }

                return _products;
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

        public static bool SkipCheckUMTime
        {
            get
            {
                return GetValue("SkipCheckUMTime").Equals("1");
            }
        }

        public static string SystemName
        {
            get
            {
                return GetSettingValue(new SystemSettingsSelByNameCriteria { Name = "SystemName" });
            }
        }

        public static string SystemDisplayName
        {
            get
            {
                var displayName = GetSettingValue(new SystemSettingsSelByNameCriteria { Name = "SystemDisplayName" });

                if (string.IsNullOrEmpty(displayName))
                {
                    return SystemName;
                }

                return displayName;
            }
        }

        public static DateTime SystemStartDate
        {
            get
            {
                if (_systemStartDate == null)
                {
                    var systemSetting = SystemSetting.GetSystemSetting(new SystemSettingsSelByNameCriteria
                        {
                            Name = "SystemStartDate"
                        });

                    _systemStartDate = string.IsNullOrEmpty(systemSetting.SValue)
                                           ? SystemTimeService.GetSystemDateTime()
                                           : Convert.ToDateTime(systemSetting.SValue);
                }

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

        public static DateTime UMEndTime
        {
            get
            {
                return Convert.ToDateTime(GetValue("UM_EndTime"));
            }
        }

        public static DateTime UMStartTime
        {
            get
            {
                return Convert.ToDateTime(GetValue("UM_StartTime"));
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
            return SystemSetting.GetActiveCommGroups(criteria);
        }

        public static IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria)
        {
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