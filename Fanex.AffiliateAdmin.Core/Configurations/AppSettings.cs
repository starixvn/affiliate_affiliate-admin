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

using System.Diagnostics.CodeAnalysis;

namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Criterias.Configuration.Product;
    using Criterias.Configuration.SystemSetting;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Services;
    using Services.Interface;
    using Session;

    /// <summary>
    /// AppSettings class.
    /// </summary>
    public static class AppSettings
    {
        #region Define variable

        private static readonly ISystemSettingService _systemSetting = new SystemSettingService();
        private static IEnumerable<Language> _baseLanguage = _systemSetting.GetLanguageList(new LanguageCriteria { Type = "Affiliate" });
        private static IEnumerable<Language> _affLanguage = _systemSetting.GetAffLanguageList(new AffLanguageCriteria());
        private static IEnumerable<Language> _affSupportLanguage = _systemSetting.GetAffSupportLanguageList(new AffiliateLanguageCriteria());
        private static IEnumerable<Language> _memberSiteSupportedLanguage = _systemSetting.GetMemberSiteSupportedLanguageList(new MemberSiteLanguageCriteria
            {
                Environment = Environment
            });
        private static IEnumerable<Exchange> _affiliateCurrencies = _systemSetting.GetExchangeList(new ExchangeCriteria { Type = "Affiliate" });
        private static IEnumerable<AffType> _affType = _systemSetting.GetAffTypeList(new AffTypeCriteria());
        private static Exchange _baseExchange = _systemSetting.GetSystemBaseExchange(new SystemSettingExchangeCriteria());
        private static IEnumerable<Currency> _affSystemSupportedCurrencies;
        private static IEnumerable<Currency> _affSystemAllSupportedCurrencies;
        private static IEnumerable<Currency> _playerCurrencies;
        private static DateTime? _systemStartDate;
        private static IEnumerable<DeductionType> _deductionTypes;
        private static IEnumerable<Product> _products;
        private static int? _licenseeId;

        #endregion Define variable

        public static int LicenseeID
        {
            get
            {
                if (_licenseeId == null)
                {
                    var licensee = _systemSetting.GetSystemSetting(new SystemSettingDetailCriteria()
                    {
                        Name = "LicenseeID",
                        Type = "Meta",
                        SearchBy = SearchByType.ByName,
                    });

                    _licenseeId = int.Parse(licensee.SValue);
                }

                return _licenseeId.Value;
            }
        }

        public static IEnumerable<Product> Products
        {
            get { return _products ?? (_products = _systemSetting.GetProductList(new ProductSelectCriteria())); }
        }

        public static IEnumerable<DeductionType> DeductionTypes
        {
            get
            {
                return _deductionTypes ?? (_deductionTypes = _systemSetting.GetDeductionTypes(new DeductionTypeCriteria()));
            }
        }

        public static DateTime SystemStartDate
        {
            get
            {
                if (_systemStartDate == null)
                {
                    var systemSetting = _systemSetting.GetSystemSetting(new SystemSettingDetailCriteria
                        {
                            SearchBy = SearchByType.ByName,
                            Name = "SystemStartDate"
                        });

                    _systemStartDate = string.IsNullOrEmpty(systemSetting.SValue)
                                           ? DateTime.Now
                                           : Convert.ToDateTime(systemSetting.SValue);
                }

                return _systemStartDate.Value;
            }
        }

        public static IEnumerable<Currency> AffSystemSupportedCurrencies
        {
            get
            {
                if (_affSystemSupportedCurrencies == null)
                {
                    var criteria = new AccountCriteria();
                    criteria.UserId = SessionManager.CurrentSession.CustID;
                    _affSystemSupportedCurrencies = _systemSetting.GetAffSystemSupportedCurrencyList(criteria);
                }

                return _affSystemSupportedCurrencies;
            }
        }

        public static IEnumerable<Currency> AffSystemAllSupportedCurrencies
        {
            get
            {
                if (_affSystemAllSupportedCurrencies == null)
                {
                    var criteria = new AccountCriteria();
                    criteria.UserId = SessionManager.CurrentSession.CustID;
                    _affSystemAllSupportedCurrencies = _systemSetting.GetAffSystemAllSupportedCurrencyList(criteria);
                }

                return _affSystemAllSupportedCurrencies;
            }
        }

        public static IEnumerable<Currency> PlayerCurrencies
        {
            get
            {
                _playerCurrencies = _playerCurrencies ?? _systemSetting.GetPlayerCurrencyList(new PlayerCurrencyCriteria());

                return _playerCurrencies;
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

        public static string Layout
        {
            get
            {
                return GetValue("Layout");
            }
        }

        public static int PageSize
        {
            get
            {
                return Convert.ToInt32(GetValue("PageSize"));
            }
        }

        public static int ServerId
        {
            get
            {
                return Convert.ToInt32(GetValue("ServerID"));
            }
        }

        public static string DefaultLanguage
        {
            get
            {
                return GetValue("DefaultLanguage");
            }
        }

        public static string ITMail
        {
            get
            {
                return GetValue("ITMail");
            }
        }

        public static string AffiliateName
        {
            get
            {
                return GetValue("AffiliateName");
            }
        }

        public static string MailContact
        {
            get
            {
                return GetValue("MailContact");
            }
        }

        public static Uri LBUrl
        {
            get
            {
                return string.IsNullOrEmpty(GetValue("LBUrl")) ? null : new Uri(GetValue("LBUrl"));
            }
        }

        public static bool LocalTest
        {
            get
            {
                return Environment.Equals("Local Development", StringComparison.OrdinalIgnoreCase);
            }
        }

        public static string Environment
        {
            get
            {
                return GetValue("Environment");
            }
        }

        public static bool SkipCheckUMTime
        {
            get
            {
                return GetValue("SkipCheckUMTime").Equals("1");
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public static DateTime UM_StartTime
        {
            get
            {
                return Convert.ToDateTime(GetValue("UM_StartTime"));
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public static DateTime UM_EndTime
        {
            get
            {
                return Convert.ToDateTime(GetValue("UM_EndTime"));
            }
        }

        public static string Theme
        {
            get
            {
                return GetValue("Theme");
            }
        }

        public static int TransferDay
        {
            get
            {
                return Convert.ToInt32(GetValue("TransferDay"));
            }
        }

        public static bool TestMonthlyTransfer
        {
            get
            {
                return Convert.ToInt32(GetValue("TestMonthlyTransfer")) > 0;
            }
        }

        public static IEnumerable<Language> BaseLanguages
        {
            get
            {
                return _baseLanguage;
            }
        }

        public static IEnumerable<Exchange> AffiliateCurrencies
        {
            get
            {
                return _affiliateCurrencies;
            }
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

        public static IEnumerable<Language> MemberSiteSiteSupportedLanguages
        {
            get
            {
                return _memberSiteSupportedLanguage;
            }
        }

        public static IEnumerable<AffType> AffTypes
        {
            get
            {
                return _affType;
            }
        }

        public static Exchange BaseExchange
        {
            get
            {
                return _baseExchange;
            }
        }

        public static decimal MinimumWithdrawalAmount
        {
            get
            {
                return Convert.ToDecimal(GetValue("MinimumWithdrawalAmount"));
            }
        }

        public static string SystemName
        {
            get
            {
                return GetSettingValue(new SystemSettingDetailCriteria { SearchBy = SearchByType.ByName, Name = "SystemName" });
            }
        }

        public static Uri IPCheckerUrl
        {
            get
            {
                return new Uri(GetValue("IPCheckerURL"));
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

        public static int ViewLogBackMonth
        {
            get
            {
                return Convert.ToInt32(GetValue("ViewLogBackMonth"));
            }
        }

        public static int LeftString
        {
            get
            {
                return Convert.ToInt32(GetValue("LeftString"));
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

        public static int PastNthMonth
        {
            get
            {
                return int.Parse(GetValue("PastNthMonth"));
            }
        }

        public static bool EnableTransformer
        {
            get
            {
                return int.Parse(GetValue("EnableTransformer")) == 1;
            }
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

        /// <summary>
        /// Get Setting value.
        /// </summary>
        /// <param name="detailCriteria">System Setting Detail Criteria.</param>
        /// <returns>Setting value.</returns>
        public static string GetSettingValue(SystemSettingDetailCriteria detailCriteria)
        {
            string value = string.Empty;
            var systemSettings = _systemSetting.GetSystemSettingList(detailCriteria) as List<SystemSetting>;

            if (systemSettings != null && systemSettings.Count > 0)
            {
                var selectedSystemSetting = systemSettings.FirstOrDefault(x => x.Environment == Environment);

                if (selectedSystemSetting != null)
                {
                    value = selectedSystemSetting.SValue;
                }
            }

            return value;
        }

        public static IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria)
        {
            return _systemSetting.GetCommGroups(criteria);
        }

        public static IEnumerable<CommGroup> GetActiveCommGroups(CommGroupSelActiveListCriteria criteria)
        {
            return _systemSetting.GetActiveCommGroups(criteria);
        }
    }
}