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

        private static readonly ISystemSettingService _SystemSetting;
        private static IEnumerable<Language> _baseLanguage;
        private static IEnumerable<Language> _affLanguage;
        private static IEnumerable<Language> _affSupportLanguage;
        private static IEnumerable<Language> _memberSiteSupportedLanguage;
        private static IEnumerable<Exchange> _affiliateCurrencies;
        private static IEnumerable<AffType> _affType;
        private static IEnumerable<Currency> _affSystemSupportedCurrencies;
        private static IEnumerable<Currency> _affSystemAllSupportedCurrencies;
        private static IEnumerable<Currency> _playerCurrencies;
        private static DateTime _systemStartDate;
        private static List<SelectListItem> _baseCurrencyInAffiliate;
        private static List<SelectListItem> _baseCurrencyInMember;
        private static IEnumerable<DeductionType> _deductionTypes;
        private static IEnumerable<Product> _products;
        private static Exchange _baseExchange;
        private static int _licenseeID;

        #endregion Define variable

        static AppSettings()
        {
            _SystemSetting = new SystemSettingService();
            var licensee = _SystemSetting.GetSystemSetting(new SystemSettingDetailCriteria()
            {
                Name = "LicenseeID",
                Type = "Meta",
                SearchBy = SearchByType.ByName,
            });

            _licenseeID = int.Parse(licensee.SValue);
            _baseLanguage = _SystemSetting.GetLanguageList(new LanguageCriteria { Type = "Affiliate" });
            _affLanguage = _SystemSetting.GetAffLanguageList(new AffLanguageCriteria());
            _affSupportLanguage = _SystemSetting.GetAffSupportLanguageList(new AffiliateLanguageCriteria());
            _memberSiteSupportedLanguage = _SystemSetting.GetMemberSiteSupportedLanguageList(new MemberSiteLanguageCriteria
            {
                Environment = Environment
            });
            _affiliateCurrencies = _SystemSetting.GetExchangeList(new ExchangeCriteria { Type = "Affiliate" });
            _affType = _SystemSetting.GetAffTypeList(new AffTypeCriteria());
            var systemSetting = _SystemSetting.GetSystemSetting(new SystemSettingDetailCriteria
                {
                    SearchBy = SearchByType.ByName,
                    Name = "SystemStartDate"
                });
            _systemStartDate = string.IsNullOrEmpty(systemSetting.SValue) ? DateTime.Now : Convert.ToDateTime(systemSetting.SValue);
            _baseExchange = _SystemSetting.GetSystemBaseExchange(new SystemSettingExchangeCriteria());
        }

        public static int LicenseeID
        {
            get
            {
                return _licenseeID;
            }
        }

        public static IEnumerable<Product> Products
        {
            get
            {
                if (_products == null)
                {
                    _products = _SystemSetting.GetProductList(new ProductSelectCriteria());
                }

                return _products;
            }
        }

        public static IEnumerable<DeductionType> DeductionTypes
        {
            get
            {
                if (_deductionTypes == null)
                {
                    _deductionTypes = _SystemSetting.GetDeductionTypes(new DeductionTypeCriteria());
                }

                return _deductionTypes;
            }
        }

        public static DateTime SystemStartDate
        {
            get
            {
                return _systemStartDate;
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
                    _affSystemSupportedCurrencies = _SystemSetting.GetAffSystemSupportedCurrencyList(criteria);                    
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
                    _affSystemAllSupportedCurrencies = _SystemSetting.GetAffSystemAllSupportedCurrencyList(criteria);
                }

                return _affSystemAllSupportedCurrencies;
            }
        }
       
        public static IEnumerable<Currency> PlayerCurrencies
        {
            get
            {
                _playerCurrencies = _playerCurrencies ?? _SystemSetting.GetPlayerCurrencyList(new PlayerCurrencyCriteria());

                return _playerCurrencies;
            }
        }

        public static List<SelectListItem> BaseCurrencyInAffiliate
        {
            get
            {
                if (_baseCurrencyInAffiliate == null)
                {
                    _baseCurrencyInAffiliate = new List<SelectListItem> 
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

                return _baseCurrencyInAffiliate;
            }
        }

        public static List<SelectListItem> BaseCurrencyInMember
        {
            get
            {
                if (_baseCurrencyInMember == null)
                {
                    _baseCurrencyInMember = new List<SelectListItem> 
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

                return _baseCurrencyInMember;
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

        public static string LBUrl
        {
            get
            {
                return GetValue("LBUrl");
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

        public static DateTime UM_StartTime
        {
            get
            {
                return Convert.ToDateTime(GetValue("UM_StartTime"));
            }
        }

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

        public static string IPCheckerURL
        {
            get
            {
                return GetValue("IPCheckerURL");
            }
        }

        public static string InternalURL
        {
            get
            {
                return GetValue("InternalURL");
            }
        }

        public static string InternalURLKiosk
        {
            get
            {
                return GetValue("InternalURL_Kiosk");
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
            var systemSettings = _SystemSetting.GetSystemSettingList(detailCriteria) as List<SystemSetting>;

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
            return _SystemSetting.GetCommGroups(criteria);
        }
    }
}