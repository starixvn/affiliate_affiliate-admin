namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.AffType;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Country;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Language;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Product;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Data.Repository;

    public interface ISystemSettingService
    {
        #region Configuration

        /// <summary>
        /// Get system setting.
        /// For case, search by ID.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return object.</returns>
        SystemSetting GetSystemSetting(ICriteria criteria);

        /// <summary>
        /// Gets list setting when search by name, type.
        /// </summary>
        /// <param name="detailCriteria">Input criteria.</param>
        /// <returns>Return list system setting.</returns>
        IEnumerable<SystemSetting> GetSystemSettingList(ICriteria detailCriteria);

        IEnumerable<SystemSetting> GetSystemSettingList(SystemSettingSelectCriteria criteria);

        void UpdateSystemSetting(SystemSettingUpdateCriteria criteria);

        void DeleteSystemSetting(SystemSettingDeleteCriteria criteria);

        void InsertSystemSetting(SystemSettingAddCriteria criteria);

        #endregion

        #region Language

        /// <summary>
        /// Gets Affiliate language.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return list language.</returns>
        IEnumerable<Language> GetLanguageList(AdminSupportedLanguagesSelCriteria criteria);

        IEnumerable<Language> GetAffLanguageList(AffLanguageCriteria criteria);

        IEnumerable<Language> GetAffSupportLanguageList(AffiliateLanguageCriteria criteria);

        IEnumerable<Language> GetMemberSiteSupportedLanguageList(MemberSiteLanguageCriteria criteria);

        IEnumerable<Language> GetLanguageList(LanguageSelectCriteria criteria);

        Language GetLanguage(LanguageDetailCriteria criteria);

        void UpdateLanguage(LanguageUpdateCriteria criteria);

        void InsertLanguage(LanguageAddCriteria criteria);

        void DeleteLanguage(LanguageDeleteCriteria criteria);

        #endregion

        #region Country

        /// <summary>
        /// Gets Affiliate Country.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return list Country.</returns>
        IEnumerable<Country> GetCountryList(CountrySelectCriteria criteria);

        Country GetCountry(CountryDetailCriteria criteria);

        void UpdateCountry(CountryUpdateCriteria criteria);

        void InsertCountry(CountryAddCriteria criteria);

        void DeleteCountry(CountryDeleteCriteria criteria);

        #endregion

        #region Product

        IEnumerable<Product> GetProductList(ProductSelectCriteria criteria);

        Product GetProduct(ProductDetailCriteria criteria);

        void UpdateProduct(ProductUpdateCriteria criteria);

        void InsertProduct(ProductAddCriteria criteria);

        void DeleteProduct(ProductDeleteCriteria criteria);

        #endregion

        #region Affiliate Type

        IEnumerable<AffType> GetAffTypeList(AffTypeSelectCriteria criteria);

        AffType GetAffType(AffTypeDetailCriteria criteria);

        void UpdateAffType(AffTypeUpdateCriteria criteria);

        void InsertAffType(AffTypeAddCriteria criteria);

        void DeleteAffType(AffTypeDeleteCriteria criteria);

        #endregion

        /// <summary>
        /// Gets Deduction Types.
        /// </summary>
        /// <param name="criteria">Deduction Type Criteria.</param>
        /// <returns>Return List Deduction Type.</returns>
        IEnumerable<DeductionType> GetDeductionTypes(DeductionTypeCriteria criteria);

        /// <summary>
        /// Gets Affiliate Exchange.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return list Exchange.</returns>
        IEnumerable<Exchange> GetExchangeList(AffiliateCurrenciesSelCriteria criteria);

        /// <summary>
        /// Gets Affiliate Currency.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return list Currency.</returns>
        IEnumerable<Currency> GetPlayerCurrencyList(PlayerCurrencyCriteria criteria);

        /// <summary>
        /// Get system supported currencies.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Currency list.</returns>
        IEnumerable<Currency> GetAffSystemSupportedCurrencyList(SystemSupportedCurrencyCriteria criteria);

        IEnumerable<Currency> GetLicenseeSupportedCurrencyList(LicenseeSupportedCurrencyCriteria criteria);

        /// <summary>
        /// Get system supported currencies (without currency permission).
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Currency list.</returns>
        IEnumerable<Currency> GetAffSystemAllSupportedCurrencyList(AllSystemCurrencyCriteria criteria);

        IEnumerable<AffType> GetAffTypeList(AffTypeCriteria criteria);

        IEnumerable<CommGroup> GetCommGroups(CommGroupCurrencyCriteria criteria = null);

        IEnumerable<CommGroup> GetActiveCommGroups(CommGroupSelActiveListCriteria criteria);

        Exchange GetSystemBaseExchange(SystemSettingExchangeCriteria criteria);

        /// <summary>
        /// Get payment method.
        /// </summary>
        /// <param name="criteria">Payment method criteria.</param>
        /// <returns>List system setting payment method.</returns>
        IEnumerable<SystemSetting> GetPaymentMethod(PaymentMethodCriteria criteria);

        /// <summary>
        /// Get customer pages.
        /// </summary>
        /// <param name="criteria">System setting detail criteria.</param>
        /// <returns>List system setting.</returns>
        IEnumerable<SystemSetting> GetCustomerPages(SystemSettingsSelByTypeCriteria criteria);

        /// <summary>
        /// Get Affiliate System DateTime.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return System DateTime.</returns>
        DateTime GetSystemDateTime();
    }
}