// --------------------------------------------------------------------------
// <copyright file="SystemSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.25">Created</change>
//   <change who="Muller.Nguyen" date="2013.07.04">Add properties ListCurrency</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Services
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.AffType;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Country;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Language;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Product;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Helper;

    /// <summary>
    /// System setting service class.
    /// </summary>
    public class SystemSettingService : QueryService, ISystemSettingService
    {
        #region Configuration

        /// <summary>
        /// Gets object setting.
        /// </summary>
        /// <param name="detailCriteria">Input criteria.</param>
        /// <returns>Return object.</returns>
        public SystemSetting GetSystemSetting(SystemSettingDetailCriteria detailCriteria)
        {
            return this.GetEntity<SystemSetting>(detailCriteria);
        }

        /// <summary>
        /// Gets list setting.
        /// </summary>
        /// <param name="detailCriteria">Input criteria.</param>
        /// <returns>Return list system setting.</returns>
        public IEnumerable<SystemSetting> GetSystemSettingList(SystemSettingDetailCriteria detailCriteria)
        {
            return this.GetDataOutput<SystemSetting>(detailCriteria);
        }

        public IEnumerable<SystemSetting> GetSystemSettingList(SystemSettingSelectCriteria criteria)
        {
            return this.GetDataOutput<SystemSetting>(criteria);
        }
       
        public void UpdateSystemSetting(SystemSettingUpdateCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void DeleteSystemSetting(SystemSettingDeleteCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void InsertSystemSetting(SystemSettingAddCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        #endregion

        #region Language

        /// <summary>
        /// Gets Affiliate language.
        /// </summary>
        /// <param name="criteria">Language criteria.</param>
        /// <returns>Return List Language.</returns>
        public IEnumerable<Language> GetLanguageList(LanguageCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<Language>(criteria);
            return list;
        }

        /// <summary>
        /// List Affiliate Language.
        /// </summary>
        /// <param name="criteria">Affiliate Language Criteria.</param>
        /// <returns>Return list affiliate language.</returns>
        public IEnumerable<Language> GetAffLanguageList(AffLanguageCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<Language>(criteria);
            return list;
        }

        public IEnumerable<Language> GetAffSupportLanguageList(AffiliateLanguageCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<Language>(criteria);
            return list;
        }

        /// <summary>
        /// Get the language list of member site.
        /// </summary>
        /// <param name="criteria">The criteria object.</param>
        /// <returns>The member site language list.</returns>
        public IEnumerable<Language> GetMemberSiteSupportedLanguageList(MemberSiteLanguageCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<Language>(criteria);
            return list;
        }

        /// <summary>
        /// Get the supported language list.
        /// </summary>
        /// <param name="criteria">The criteria object.</param>
        /// <returns>The supported language list.</returns>
        public IEnumerable<Language> GetLanguageList(LanguageSelectCriteria criteria)
        {
            return this.GetDataOutput<Language>(criteria);
        }

        public Language GetLanguage(LanguageDetailCriteria criteria)
        {
            return this.GetEntity<Language>(criteria);
        }

        public void UpdateLanguage(LanguageUpdateCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void InsertLanguage(LanguageAddCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void DeleteLanguage(LanguageDeleteCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        #endregion

        #region Country

        /// <summary>
        /// Gets Affiliate Country.
        /// </summary>
        /// <param name="criteria">Country Criteria.</param>
        /// <returns>Return List Countries.</returns>
        public IEnumerable<Country> GetCountryList(CountrySelectCriteria criteria)
        {
            return this.GetDataOutput<Country>(criteria);
        }

        public Country GetCountry(CountryDetailCriteria criteria)
        {
            return this.GetEntity<Country>(criteria);
        }

        public void UpdateCountry(CountryUpdateCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void InsertCountry(CountryAddCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void DeleteCountry(CountryDeleteCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        #endregion

        #region Product

        public IEnumerable<Product> GetProductList(ProductSelectCriteria criteria)
        {
            return this.GetDataOutput<Product>(criteria);
        }

        public Product GetProduct(ProductDetailCriteria criteria)
        {
            return this.GetEntity<Product>(criteria);
        }

        public void UpdateProduct(ProductUpdateCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void InsertProduct(ProductAddCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void DeleteProduct(ProductDeleteCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        #endregion

        #region Affiliate Type

        public IEnumerable<AffType> GetAffTypeList(AffTypeSelectCriteria criteria)
        {
            return this.GetDataOutput<AffType>(criteria);
        }

        public AffType GetAffType(AffTypeDetailCriteria criteria)
        {
            return this.GetEntity<AffType>(criteria);
        }

        public void UpdateAffType(AffTypeUpdateCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void InsertAffType(AffTypeAddCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        public void DeleteAffType(AffTypeDeleteCriteria criteria)
        {
            this.ExecuteOutput(criteria);
        }

        #endregion        

        /// <summary>
        /// Gets Deduction Types.
        /// </summary>
        /// <param name="criteria">Deduction Type Criteria.</param>
        /// <returns>Return List Deduction Type.</returns>
        public IEnumerable<DeductionType> GetDeductionTypes(DeductionTypeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<DeductionType>(criteria);
            return list;
        }

        /// <summary>
        /// Gets Affiliate Country.
        /// </summary>
        /// <param name="criteria">Exchange Criteria.</param>
        /// <returns>Return List Exchange.</returns>
        public IEnumerable<Exchange> GetExchangeList(ExchangeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<Exchange>(criteria);
            return list;
        }

        /// <summary>
        /// Gets Affiliate Country.
        /// </summary>
        /// <param name="criteria">Currency Criteria.</param>
        /// <returns>Return List Currency.</returns>
        public IEnumerable<Currency> GetPlayerCurrencyList(PlayerCurrencyCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<Currency>(criteria);
            return list;
        }

        public IEnumerable<AffType> GetAffTypeList(AffTypeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return this.GetData<AffType>(criteria);
        }

        public IEnumerable<Currency> GetAffSystemSupportedCurrencyList(AccountCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            criteria.Type = "SystemCurrency";

            return this.GetData<Currency>(criteria);
        }

        public IEnumerable<Currency> GetAffSystemAllSupportedCurrencyList(AccountCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            criteria.Type = "AllSystemCurrency";

            return this.GetData<Currency>(criteria);
        }

        /// <summary>
        /// Get the list of commission groups.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List commission groups.</returns>
        public IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria)
        {
            if (criteria == null)
            {
                criteria = new CommGroupCurrencyCriteria { Currency = "All" };
            }

            criteria.UserId = SessionManager.CurrentSession.CustID;

            return GetData<CommGroup>(criteria);            
        }

        public IEnumerable<CommGroup> GetActiveCommGroups(CommGroupSelActiveListCriteria criteria)
        {
            return GetData<CommGroup>(criteria);
        }

        public Exchange GetSystemBaseExchange(SystemSettingExchangeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetEntity<Exchange>(criteria);
        }

        /// <summary>
        /// Get payment method.
        /// </summary>
        /// <param name="criteria">Payment method criteria.</param>
        /// <returns>List system setting payment method.</returns>
        public IEnumerable<SystemSetting> GetPaymentMethod(PaymentMethodCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return this.GetData<SystemSetting>(criteria);
        }

        /// <summary>
        /// Get customer pages.
        /// </summary>
        /// <param name="criteria">System setting detail criteria.</param>
        /// <returns>List system setting.</returns>
        public IEnumerable<SystemSetting> GetCustomerPages(SystemSettingDetailCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return this.GetData<SystemSetting>(criteria);
        }
    }
}