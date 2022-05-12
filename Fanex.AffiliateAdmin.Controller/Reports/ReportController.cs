namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.SessionState;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Constants;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Options;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Models.Reports;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [FunctionAuth(Function = FunctionPolicyNames.AffReport, CurrenciesRequired = true)]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly ISystemSettings _systemSettings;
        private readonly ICommGroupService _commGroupService;

        private ICurrentSession CurrentSession => SessionManager.CurrentSession;

        public ReportController(
            IAppSetting appSetting,
            IReportService reportService,
            ISystemSettings systemSettings,
            ICommGroupService commGroupService) : base(appSetting)
        {
            _reportService = reportService;
            _systemSettings = systemSettings;
            _commGroupService = commGroupService;
        }

        protected ISystemSettings SystemSettings => _systemSettings;
        protected IReportService ReportService => _reportService;

        #region Company Win Loss Report.

        /// <summary>
        /// Company Win Loss Report.
        /// </summary>
        /// <returns>Company win loss page.</returns>
        [HttpGet]
        public ActionResult CompanyWinloss()
        {
            var products = SystemSettings.Products;

            var model = new CompanyWinlossViewModel
            {
                ProductOptionList = GetProductOptionList(products),
                PreferLanguageOptionList = GetPreferLanguageOptionList(),
                PlayerCurrencyOptionList = GetPlayerCurrencyOptionList(),
                CommissionGroupOptionList = GetCommissionGroupOptionList(),
                BaseCurrencyOptionList = SystemSettings.GetBaseCurrencyOptionList(true),
                AffTypeOptionList = GetAffTypeOptionList(),
                DrawFilterOptionList = SelectListHelpers.GetCommonOptionList(OptionManager.DrawFilters, DefaultValues.Int.ToString()),
                GroupByOptionList = SelectListHelpers.GetCommonOptionList(OptionManager.GroupByListByDateMonth, DefaultValues.Int.ToString())
            };

            ViewBag.Products = GetProductsSerializer(products);

            return View(model);
        }

        /// <summary>
        /// Company Win loss Report for specific affiliate.
        /// </summary>
        /// <param name="model">Company View Model.</param>
        /// <returns>Company win loss page.</returns>
        [HttpGet]
        public ActionResult CompanyWinlossByAffiliate(CompanyWLByAffiliateViewModel model)
        {
            var currencyCode = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).CurrencyCode;
            var baseCurrencyList = GetBaseCurrencyInMemberList(currencyCode);
            var products = SystemSettings.Products;

            model.ProductOptionList = GetProductOptionList(products, model.Products);
            model.PlayerCurrencyOptionList = GetPlayerCurrencyOptionList(true, model.PlayerCurrencies);
            model.BaseCurrencyOptionList = GetBaseCurrencyOptionByCookiesList(model.BaseCurrency, baseCurrencyList, currencyCode);
            model.GroupByOptionList = SelectListHelpers.GetCommonOptionList(OptionManager.GroupByListByDateMonth, model.GroupBy.ToString());
            model.DrawFilterOptionList = SelectListHelpers.GetCommonOptionList(OptionManager.DrawFilters, model.DrawFilter.ToString());

            ViewBag.Products = GetProductsSerializer(products);
            ViewBag.AffiliateCurrencySymbol = FormaterCurrency.GetCurrencySymbol(currencyCode);

            return View("CompanyWinlossByAffiliate", model);
        }

        /// <summary>
        /// Index Page.
        /// </summary>
        /// <returns>Redirect to another action.</returns>
        public ActionResult Index()
        {
            return RedirectToAction("CompanyWinloss");
        }

        #endregion Company Win Loss Report.

        #region Member Win Loss Report

        /// <summary>
        /// Member Win loss Report.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Member Win loss page.</returns>
        [HttpGet]
        public ActionResult MemberWinloss(MemberWinlossViewModel model)
        {
            var products = SystemSettings.Products;
            model.ProductOptionList = GetProductOptionList(products, model.Products);
            model.BaseCurrencyOptionList = SystemSettings.GetBaseCurrencyOptionList(true);
            model.PlayerCurrencyOptionList = GetPlayerCurrencyOptionList(true, model.PlayerCurrencies);
            model.PreferLanguageOptionList = GetPreferLanguageOptionList(true, model.PreferLanguages);
            model.CommissionGroupOptionList = GetCommissionGroupOptionList(true, model.CommGroups);
            model.AffTypeOptionList = GetAffTypeOptionList(true, model.AffTypes);
            model.GroupByOptionList = SelectListHelpers.GetCommonOptionList(OptionManager.GroupByListByDateMember, model.GroupBy.ToString());
            model.DrawFilterOptionList = SelectListHelpers.GetCommonOptionList(OptionManager.DrawFilters, model.DrawFilter.ToString());

            ViewBag.Products = GetProductsSerializer(products);

            return View("MemberWinloss", model);
        }

        /// <summary>
        /// Member Win loss Report for specific affiliate.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Member win loss page.</returns>
        [HttpGet]
        public ActionResult MemberWinlossByAffiliate(MemberWinlossByAffiliateViewModel model)
        {
            var products = SystemSettings.Products;
            var currencyCode = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).CurrencyCode;
            var baseCurrencyList = GetBaseCurrencyInMemberList(currencyCode);

            model.ProductOptionList = GetProductOptionList(products, model.Products);
            model.PlayerCurrencyOptionList = GetPlayerCurrencyOptionList(true, model.PlayerCurrencies);
            model.BaseCurrencyOptionList = SelectListHelpers.GetCommonOptionList(baseCurrencyList, model.BaseCurrency.ToString());
            model.DrawFilterOptionList = SelectListHelpers.GetCommonOptionList(OptionManager.DrawFilters, model.DrawFilter.ToString());
            model.GroupByOptionList = SelectListHelpers.GetCommonOptionList(OptionManager.GroupByListByDateMember, model.GroupBy.ToString());

            ViewBag.Products = GetProductsSerializer(products);
            ViewBag.AffiliateCurrencySymbol = FormaterCurrency.GetCurrencySymbol(currencyCode);

            return View("MemberWinlossByAffiliate", model);
        }

        #endregion Member Win Loss Report

        #region Member Statistics

        [HttpGet]
        public ActionResult MemberStatistics()
        {
            var model = new MemberStatisticsViewModel
            {
                PreferLanguageOptionList = SelectListHelpers.GetLanguageOptionList(SystemSettings.AffSupportLanguages),
                AffiliateTypeOptionList = SelectListHelpers.GetAffiliateTypeOptionList(SystemSettings.AffTypes),
                CurrencyOptionList = SelectListHelpers.GetPlayerCurrencyOptionList(CurrentSession.CurrencyList)
            };

            return View(model);
        }

        /// <summary>
        /// Get data for report.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Report Data result.</returns>
        [HttpPost]
        public ActionResult MemberStatistics(MemberStatisticReportCriteria criteria)
        {
            criteria.IsITAdmin = CurrentSession.IsIT;
            criteria.UserCurrencies = CurrentSession.BuildUserCurrencyParam();

            if (criteria.GroupBy == 2)
            {
                // If group by is month, we need to first day and last day of month.
                criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, SystemSettings.SystemStartDate);
                criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, SystemSettings.SystemDateTime);
            }

            var data = ReportService.GetMemberStatistics(criteria).ToList();

            return Json(new
            {
                List = data,
                CurrentPage = 1,
                ItemsPerPage = data.Count,
                TotalItem = data.Count,
                GroupBy = criteria.GroupBy.ToString(),
                SubTotal = new
                {
                    TotalClick = data.Sum(x => x.TotalClick),
                    UniqueClick = data.Sum(x => x.UniqueClick),
                    Signups = data.Sum(x => x.Signups),
                    FDM = data.Sum(x => x.FDM),
                    NonDeposit = data.Sum(x => x.NonDeposit),
                    ActivePlayers = data.Sum(x => x.ActivePlayers)
                }
            });
        }

        #endregion Member Statistics

        #region Affiliate Win Loss Report

        /// <summary>
        /// Commission Report.
        /// </summary>
        /// <param name="model">Input criteria.</param>
        /// <returns>Commission Report Page.</returns>
        [HttpGet]
        public ActionResult AffiliateWinLoss(AffiliateWinLossViewModel model)
        {
            var products = SystemSettings.Products;
            model.ProductOptionList = GetProductOptionList(products, model.Products);
            model.AffTypeOptionList = GetAffTypeOptionList(true, model.AffTypes);
            model.PreferLanguageOptionList = GetPreferLanguageOptionList(true, model.PreferLanguages);
            model.CommissionGroupOptionList = GetCommissionGroupOptionList(true, model.CommGroups);
            model.BaseCurrencyOptionList = SystemSettings.GetBaseCurrencyOptionList(false, model.BaseCurrency);

            var currencyList = CurrentSession.CurrencyList;
            var selectedCurrencyList = string.IsNullOrEmpty(model.Currencies) ? currencyList.Select(t => t.CurrencyID) : model.Currencies.Split(',').Select(t => short.Parse(t));
            model.CurrencyOptionList = new MultiSelectList(currencyList, "CurrencyID", "CurrencyCode", selectedCurrencyList);

            ViewBag.Products = GetProductsSerializer(products);
            ViewBag.HasCommGroupPolicy = CurrentSession.HasReadPolicy(FunctionPolicyNames.CommissionGroup);
            return View(model);
        }

        /// <summary>
        /// Commission By Affiliate.
        /// </summary>
        /// <param name="model">Model Input.</param>
        /// <returns>Commission By Affiliate Result.</returns>
        [HttpGet]
        public ActionResult AffiliateWinLossByAffiliate(AffiliateWLByAffiliateViewModel model)
        {
            var products = SystemSettings.Products;
            model.ProductOptionList = GetProductOptionList(products, model.Products);
            model.AffTypeOptionList = GetAffTypeOptionList();
            model.BaseCurrencyOptionList = GetBaseCurrencyOptionByCookiesList(model.BaseCurrency);

            ViewBag.Products = GetProductsSerializer(products);
            ViewBag.HasCashbackDeduction = SystemSettings.DeductionTypes.Any(x => x.ProductID == 0);

            return View(model);
        }

        /// <summary>
        /// Qualified Members.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Action Result.</returns>
        public ActionResult QualifiedMembers(QualifiedMembersReportCriteria criteria)
        {
            var products = GetValidProducts(criteria.GroupId, criteria.MonthDate, criteria.Products);
            criteria.Products = string.Join(",", products);
            criteria.UserCurrencies = CurrentSession.BuildUserCurrencyParam();

            var members = ReportService.GetQualifiedMembers(criteria);

            ViewBag.SymbolCurrency = FormaterCurrency.GetCurrencySymbol(criteria.Currency);
            ViewBag.BaseCurrency = criteria.BaseCurrency;
            ViewBag.Products = products;

            return View(members);
        }

        #endregion Affiliate Win Loss Report

        #region Affiliate Commission Report

        /// <summary>
        /// Processing Commission.
        /// </summary>
        /// <returns>Processing Commission Page.</returns>
        public ActionResult AffiliateCommission()
        {
            var products = SystemSettings.Products;
            ViewBag.Products = GetProductsSerializer(products);

            var model = new AffiliateCommissionViewModel
            {
                ProductOptionList = GetProductOptionList(products),
                AffTypeOptionList = GetAffTypeOptionList(true),
                BaseCurrencyOptionList = SystemSettings.GetBaseCurrencyOptionList(false),
                PreferLanguageOptionList = GetPreferLanguageOptionList(),
                CommissionGroupOptionList = GetCommissionGroupOptionList(),
                AffiliateCurrencyOptionList = new MultiSelectList(CurrentSession.CurrencyList, "CurrencyID", "CurrencyCode")
            };

            return View(model);
        }

        /// <summary>
        /// Commission By Affiliate.
        /// </summary>
        /// <param name="model">Model Input.</param>
        /// <returns>Commission By Affiliate Result.</returns>
        [HttpGet]
        public ActionResult AffiliateCommissionByAffiliate(AffiliateCommissionByAffiliateViewModel model)
        {
            var products = SystemSettings.Products;

            ViewBag.Products = GetProductsSerializer(products);
            ViewBag.HasCashbackDeduction = SystemSettings.DeductionTypes.Any(x => x.ProductID == 0);

            model.BaseCurrencyOptionList = GetBaseCurrencyOptionByCookiesList(model.BaseCurrency);
            model.ProductOptionList = GetProductOptionList(products, model.Products);

            return View(model);
        }

        #endregion Affiliate Commission Report

        #region Helpers

        /// <summary>
        /// Get base currency list.
        /// </summary>
        /// <returns>Base currency list.</returns>
        private List<SelectListItem> GetBaseCurrencyInMemberList(string currencyCode)
        {
            var exchange = SystemSettings.BaseExchange;
            var currencyList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "0", Text = Lang.Member },
                        new SelectListItem { Value = "1", Text = exchange.Code },
                    };

            if (!currencyCode.Equals(exchange.Code))
            {
                currencyList.Add(new SelectListItem { Value = "2", Text = currencyCode });
            }

            return currencyList;
        }

        /// <summary>
        /// Get valid products.
        /// </summary>
        /// <param name="groupId">Commission Group.</param>
        /// <param name="month">Selected Month.</param>
        /// <param name="selectedProducts">Selected Products.</param>
        /// <returns>Valid Product List.</returns>
        private IEnumerable<byte> GetValidProducts(int groupId, DateTime month, string selectedProducts)
        {
            var selectedProductList = string.IsNullOrEmpty(selectedProducts)
                                  ? new List<byte>()
                                  : selectedProducts.Split(',').Select(byte.Parse);

            if (groupId == 0)
            {
                return selectedProductList.ToList();
            }

            var detailCriteria = new TierSettingCriteria { GroupIDList = groupId.ToString(CultureInfo.InvariantCulture), Date = month };
            var productListInCommGroup = _commGroupService.GetTierSettings(detailCriteria).Select(t => t.ProductID).Distinct();

            return selectedProductList.Intersect(productListInCommGroup).ToList();
        }

        private string GetProductsSerializer(IEnumerable<Product> products)
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Serialize(products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));
        }

        private static MultiSelectList GetProductOptionList(IEnumerable<Product> productList, string products = null)
        {
            var selectedProducts = string.IsNullOrEmpty(products) ? productList.Where(t => t.IsEnabled).Select(t => t.ProductID) : products.Split(',').Select(t => byte.Parse(t));
            var translatedProducts = new List<Product>();

            foreach (var product in productList)
            {
                var name = Lang.ResourceManager.GetString(product.ProductName.Replace(" ", string.Empty));

                translatedProducts.Add(new Product
                {
                    ProductID = product.ProductID,
                    ProductName = string.IsNullOrEmpty(name) ? product.ProductName : name
                });
            }

            return new MultiSelectList(translatedProducts, "ProductID", "ProductName", selectedProducts);
        }

        private MultiSelectList GetAffTypeOptionList(bool isUsingSelectedValues = false, string selectedAffTypes = null)
        {
            var affTypeList = SystemSettings.AffTypes;

            if (isUsingSelectedValues)
            {
                var selectedAffTypeList = string.IsNullOrEmpty(selectedAffTypes) ? affTypeList.Select(t => t.TypeID) : selectedAffTypes.Split(',').Select(t => byte.Parse(t));

                return new MultiSelectList(affTypeList, "TypeID", "Name", selectedAffTypeList);
            }

            return new MultiSelectList(affTypeList, "TypeID", "Name");
        }

        private MultiSelectList GetPreferLanguageOptionList(bool isUsingSelectedValues = false, string preferLanguages = null)
        {
            var affSupportLanguages = SystemSettings.AffSupportLanguages;

            if (isUsingSelectedValues)
            {
                var selectedAffSupportLanguages = string.IsNullOrEmpty(preferLanguages) ? affSupportLanguages.Select(t => t.LanguageID) : preferLanguages.Split(',');

                return new MultiSelectList(affSupportLanguages, "LanguageID", "LanguageName", selectedAffSupportLanguages);
            }

            return new MultiSelectList(affSupportLanguages, "LanguageID", "LanguageName");
        }

        private MultiSelectList GetPlayerCurrencyOptionList(bool isUsingSelectedValues = false, string playerCurrencies = null)
        {
            var playerCurrencyList = SystemSettings.PlayerCurrencies;

            if (isUsingSelectedValues)
            {
                var selectedPlayerCurrencyList = string.IsNullOrEmpty(playerCurrencies) ? playerCurrencyList.Select(t => t.CurrencyID) : playerCurrencies.Split(',').Select(t => short.Parse(t));

                return new MultiSelectList(playerCurrencyList, "CurrencyID", "CurrencyCode", selectedPlayerCurrencyList);
            }

            return new MultiSelectList(playerCurrencyList, "CurrencyID", "CurrencyCode");
        }

        private MultiSelectList GetCommissionGroupOptionList(bool isUsingSelectedValues = false, string commGroups = null)
        {
            var commGroupList = SystemSettings.GetCommGroups();

            if (isUsingSelectedValues)
            {
                var selectedCommGroupList = string.IsNullOrEmpty(commGroups) ? commGroupList.Select(t => t.GroupID) : commGroups.Split(',').Select(t => int.Parse(t));

                return new MultiSelectList(commGroupList, "GroupID", "GroupName", selectedCommGroupList);
            }

            return new MultiSelectList(commGroupList, "GroupID", "GroupName");
        }

        private SelectList GetBaseCurrencyOptionByCookiesList(byte baseCurrency, IEnumerable<SelectListItem> baseCurrencyList = null, string currencyCode = null)
        {
            currencyCode = currencyCode ?? AffiliateInfoCookieHelper.GetAffiliate(HttpContext).CurrencyCode;
            baseCurrencyList = baseCurrencyList ?? new List<SelectListItem>
            {
                new SelectListItem { Value = "0",  Text = currencyCode }
            };

            return new SelectList(baseCurrencyList, "Value", "Text", baseCurrency);
        }

        #endregion Helpers
    }
}