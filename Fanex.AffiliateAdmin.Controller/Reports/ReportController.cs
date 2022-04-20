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
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Product;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Models;
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
        private readonly IAppSetting _appSetting;
        private readonly IReportService _reportService;
        private readonly ICommGroupService _commGroupService;

        private ICurrentSession CurrentSession => SessionManager.CurrentSession;

        public ReportController(
            IAppSetting appSetting,
            IReportService reportService,
            ICommGroupService commGroupService,
            ISystemSettingService systemSettingService) : base(systemSettingService)
        {
            _appSetting = appSetting;
            _reportService = reportService;
            _commGroupService = commGroupService;
        }

        protected IAppSetting AppSetting => _appSetting;
        protected IReportService ReportService => _reportService;

        #region Company Win Loss Report.

        /// <summary>
        /// Company Win Loss Report.
        /// </summary>
        /// <returns>Company win loss page.</returns>
        [HttpGet]
        public ActionResult CompanyWinloss()
        {
            var products = SystemSettingService.GetProductList(new ProductSelectCriteria());

            var model = new CompanyWinlossViewModel
            {
                ProductOptionList = GetProductOptionList(products),
                PreferLanguageOptionList = GetPreferLanguageOptionList(),
                PlayerCurrencyOptionList = GetPlayerCurrencyOptionList(),
                CommissionGroupOptionList = GetCommissionGroupOptionList(),
                BaseCurrencyOptionList = GetBaseCurrencyOptionList(true),
                AffTypeOptionList = GetAffTypeOptionList()
            };

            ViewBag.Products = GetProductsSerializer();

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
            ViewBag.Products = GetProductsSerializer();
            var currencyCode = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).CurrencyCode;
            ViewBag.AffiliateCurrencySymbol = FormaterCurrency.GetCurrencySymbol(currencyCode);

            var baseCurrencyList = GetBaseCurrencyInMemberList(currencyCode);

            model.AddBaseCurrencyList(baseCurrencyList);

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
            var products = SystemSettingService.GetProductList(new ProductSelectCriteria());
            model.ProductOptionList = GetProductOptionList(products, model.Products);
            model.BaseCurrencyOptionList = GetBaseCurrencyOptionList(true);
            model.PlayerCurrencyOptionList = GetPlayerCurrencyOptionList(true, model.PlayerCurrencies);
            model.PreferLanguageOptionList = GetPreferLanguageOptionList(true, model.PreferLanguages);
            model.CommissionGroupOptionList = GetCommissionGroupOptionList(true, model.CommGroups);
            model.AffTypeOptionList = GetAffTypeOptionList(true, model.AffTypes);

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
            var currencyCode = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).CurrencyCode;
            var baseCurrencyList = GetBaseCurrencyInMemberList(currencyCode);

            model.AddBaseCurrencyList(baseCurrencyList);

            ViewBag.Products = GetProductsSerializer();
            ViewBag.AffiliateCurrencySymbol = FormaterCurrency.GetCurrencySymbol(currencyCode);

            return View("MemberWinlossByAffiliate", model);
        }

        #endregion Member Win Loss Report

        #region Member Statistics

        [HttpGet]
        public ActionResult MemberStatistics()
        {
            return View(new MemberStatisticsViewModel());
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
                criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, _appSetting.SystemStartDate);
                criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, SystemTimeService.GetSystemDateTime());
            }

            var data = _reportService.GetMemberStatistics(criteria).ToList();

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
            var products = SystemSettingService.GetProductList(new ProductSelectCriteria());
            model.ProductOptionList = GetProductOptionList(products, model.Products);
            model.AffTypeOptionList = GetAffTypeOptionList(true, model.AffTypes);
            model.PreferLanguageOptionList = GetPreferLanguageOptionList(true, model.PreferLanguages);
            model.CommissionGroupOptionList = GetCommissionGroupOptionList(true, model.CommGroups);
            model.BaseCurrencyOptionList = GetBaseCurrencyOptionList(false, model.BaseCurrency);

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
            var products = SystemSettingService.GetProductList(new ProductSelectCriteria());
            model.ProductOptionList = GetProductOptionList(products, model.Products);
            model.AffTypeOptionList = GetAffTypeOptionList();
            model.BaseCurrencyOptionList = GetBaseCurrencyOptionByCookiesList(model.BaseCurrency);

            ViewBag.Products = GetProductsSerializer();
            ViewBag.HasCashbackDeduction = _appSetting.DeductionTypes.Any(x => x.ProductID == 0);

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

            var members = _reportService.GetQualifiedMembers(criteria);

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
            var products = SystemSettingService.GetProductList(new ProductSelectCriteria());
            ViewBag.Products = GetProductsSerializer(products);

            var model = new AffiliateCommissionViewModel
            {
                ProductOptionList = GetProductOptionList(products),
                AffTypeOptionList = GetAffTypeOptionList(true),
                BaseCurrencyOptionList = GetBaseCurrencyOptionList(false),
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
            var products = SystemSettingService.GetProductList(new ProductSelectCriteria());

            ViewBag.Products = GetProductsSerializer(products);
            ViewBag.HasCashbackDeduction = _appSetting.DeductionTypes.Any(x => x.ProductID == 0);

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
            var exchange = SystemSettingService.GetSystemBaseExchange(new SystemSettingExchangeCriteria());

            if (currencyCode.Equals(exchange.Code))
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem { Value = "0", Text = Lang.Member },
                        new SelectListItem { Value = "1", Text = exchange.Code },
                    };
            }
            else
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem { Value = "0", Text = Lang.Member },
                        new SelectListItem { Value = "1", Text = exchange.Code },
                        new SelectListItem { Value = "2", Text = currencyCode }
                    };
            }
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

        private string GetProductsSerializer(IEnumerable<Product> products = null)
        {
            var serializer = new JavaScriptSerializer();
            products = products ?? SystemSettingService.GetProductList(new ProductSelectCriteria());

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
            var affTypeList = SystemSettingService.GetAffTypeList(new AffTypeCriteria());

            if (isUsingSelectedValues)
            {
                var selectedAffTypeList = string.IsNullOrEmpty(selectedAffTypes) ? affTypeList.Select(t => t.TypeID) : selectedAffTypes.Split(',').Select(t => byte.Parse(t));

                return new MultiSelectList(affTypeList, "TypeID", "Name", selectedAffTypeList);
            }

            return new MultiSelectList(affTypeList, "TypeID", "Name");
        }

        private MultiSelectList GetPreferLanguageOptionList(bool isUsingSelectedValues = false, string preferLanguages = null)
        {
            var affSupportLanguages = SystemSettingService.GetAffSupportLanguageList(new AffiliateLanguageCriteria());

            if (isUsingSelectedValues)
            {
                var selectedAffSupportLanguages = string.IsNullOrEmpty(preferLanguages) ? affSupportLanguages.Select(t => t.LanguageID) : preferLanguages.Split(',');

                return new MultiSelectList(affSupportLanguages, "LanguageID", "LanguageName", selectedAffSupportLanguages);
            }

            return new MultiSelectList(affSupportLanguages, "LanguageID", "LanguageName");
        }

        private MultiSelectList GetPlayerCurrencyOptionList(bool isUsingSelectedValues = false, string playerCurrencies = null)
        {
            var playerCurrencyList = SystemSettingService.GetPlayerCurrencyList(new PlayerCurrencyCriteria());

            if (isUsingSelectedValues)
            {
                var selectedPlayerCurrencyList = string.IsNullOrEmpty(playerCurrencies) ? playerCurrencyList.Select(t => t.CurrencyID) : playerCurrencies.Split(',').Select(t => short.Parse(t));

                return new MultiSelectList(playerCurrencyList, "CurrencyID", "CurrencyCode", selectedPlayerCurrencyList);
            }

            return new MultiSelectList(playerCurrencyList, "CurrencyID", "CurrencyCode");
        }

        private MultiSelectList GetCommissionGroupOptionList(bool isUsingSelectedValues = false, string commGroups = null)
        {
            var commGroupList = SystemSettingService.GetCommGroups();

            if (isUsingSelectedValues)
            {
                var selectedCommGroupList = string.IsNullOrEmpty(commGroups) ? commGroupList.Select(t => t.GroupID) : commGroups.Split(',').Select(t => int.Parse(t));

                return new MultiSelectList(commGroupList, "GroupID", "GroupName", selectedCommGroupList);
            }

            return new MultiSelectList(commGroupList, "GroupID", "GroupName");
        }

        private SelectList GetBaseCurrencyOptionByCookiesList(byte baseCurrency, IEnumerable<SelectListItem> baseCurrencyList = null)
        {
            var affiliate = AffiliateInfoCookieHelper.GetAffiliate(HttpContext);
            baseCurrencyList = baseCurrencyList ?? new List<SelectListItem>
            {
                new SelectListItem { Value = "0",  Text = affiliate.CurrencyCode }
            };

            return new SelectList(baseCurrencyList, "Value", "Text", baseCurrency);
        }

        #endregion Helpers
    }
}