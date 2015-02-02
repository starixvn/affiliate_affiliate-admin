// --------------------------------------------------------------------------
// <copyright file="ConfigurationController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.8.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Filter;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias.Configuration.AffType;
    using Criterias.Configuration.Country;
    using Criterias.Configuration.Language;
    using Criterias.Configuration.Product;
    using Criterias.Configuration.SystemSetting;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Services;
    using Models;
    using Resources;

    /// <summary>
    /// Configuration Controller.
    /// </summary>
    [FunctionAuth(Function = "[SystemSettings]")]
    public class ConfigurationController : BaseController
    {
        #region Fields

        private readonly ISystemSettingService _systemSettingService;

        #endregion Fields

        #region Constructors

        public ConfigurationController()
            : this(null)
        {
        }

        public ConfigurationController(ISystemSettingService service)
        {
            _systemSettingService = service ?? new SystemSettingService();
        }

        #endregion Constructors

        #region Configuration

        #region Show Configuration

        /// <summary>
        /// Show configuration search.
        /// </summary>
        /// <returns>Configuration search view.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ConfigurationViewModel
            {
                TypeList = _systemSettingService.GetSystemSetting(new SystemSettingsSelByNameCriteria
                {
                    Name = "ConfigurationTypeList"
                })
            };

            return View(model);
        }

        /// <summary>
        /// Get the list of found system settings.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>The list of found system settings.</returns>
        [HttpPost]
        public ActionResult GetSystemSettingList(SystemSettingSelectCriteria criteria)
        {
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;

            var data = _systemSettingService.GetSystemSettingList(criteria);

            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        #endregion Show Configuration

        #region Update Configuration

        /// <summary>
        /// Show the configuration details.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Configuration details view.</returns>
        [HttpGet]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Accepted")]
        public ActionResult ConfigurationDetail(SystemSettingsSelByIdCriteria criteria)
        {
            var systemSetting = _systemSettingService.GetSystemSetting(criteria);

            var model = new SystemSettingUpdateViewModel
                {
                    SId = systemSetting.SId,
                    SName = systemSetting.SName,
                    SValue = systemSetting.SValue,
                    Description = systemSetting.Description,
                    Environment = systemSetting.Environment,
                    EnvironmentList = _systemSettingService.GetSystemSettingList(new SystemSettingsSelByTypeCriteria
                        {
                            Type = "Environment"
                        })
                };

            return View("ConfigurationDetail", model);
        }

        /// <summary>
        /// Update configuration.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Update configuration result.</returns>
        [HttpPost]
        public ActionResult UpdateConfiguration(SystemSettingUpdateCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;
                criteria.Language = AppSession.Language;

                _systemSettingService.UpdateSystemSetting(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return ConfigurationDetail(new SystemSettingsSelByIdCriteria { ID = criteria.ID });
        }

        #endregion Update Configuration

        #region Delete Configuration

        /// <summary>
        /// Delete configuration.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Delete configuration result.</returns>
        [HttpPost]
        public ActionResult DeleteConfiguration(SystemSettingDeleteCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.DeleteSystemSetting(criteria);
                var result = GetResultViewModel(criteria.ErrCode, "OK");

                return Json(result);
            }

            return Json(GetResultViewModel(1003));
        }

        #endregion Delete Configuration

        #region Add Configuration

        /// <summary>
        /// Show new configuration UI.
        /// </summary>
        /// <returns>New configuration UI view.</returns>
        [HttpGet]
        public ActionResult AddConfiguration()
        {
            var model = new AddConfigurationViewModel
            {
                TypeList = _systemSettingService.GetSystemSetting(new SystemSettingsSelByNameCriteria
                {
                    Name = "ConfigurationTypeList",
                }),
                EnvironmentList = _systemSettingService.GetSystemSettingList(new SystemSettingsSelByTypeCriteria
                {
                    Type = "Environment"
                })
            };

            return View(model);
        }

        /// <summary>
        /// Save new configuration.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Add configuration result.</returns>
        [HttpPost]
        public ActionResult AddConfiguration(SystemSettingAddCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;
                criteria.Language = AppSession.Language;

                _systemSettingService.InsertSystemSetting(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return AddConfiguration();
        }

        #endregion Add Configuration

        #endregion Configuration

        #region Country

        /// <summary>
        /// Show country search.
        /// </summary>
        /// <returns>Country search view.</returns>
        [HttpGet]
        public ActionResult Country()
        {
            return View("Country", new CountryViewModel());
        }

        /// <summary>
        /// Get the list of found countries.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>The list of found countries.</returns>
        [HttpPost]
        public ActionResult GetCountryList(CountrySelectCriteria criteria)
        {
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;

            var data = _systemSettingService.GetCountryList(criteria);

            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        #region Update Country

        /// <summary>
        /// Show country details.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Country details view.</returns>
        [HttpGet]
        public ActionResult CountryDetail(CountryDetailCriteria criteria)
        {
            return View("CountryDetail", _systemSettingService.GetCountry(criteria));
        }

        /// <summary>
        /// Update country.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Update country result.</returns>
        [HttpPost]
        public ActionResult UpdateCountry(CountryUpdateCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.UpdateCountry(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return CountryDetail(new CountryDetailCriteria { ID = criteria.CountryID });
        }

        #endregion Update Country

        #region Add Country

        /// <summary>
        /// Show new country UI.
        /// </summary>
        /// <returns>New country UI view.</returns>
        [HttpGet]
        public ActionResult AddCountry()
        {
            return View(new Country());
        }

        /// <summary>
        /// Save new country.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Add country result.</returns>
        [HttpPost]
        public ActionResult AddCountry(CountryAddCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                criteria.CountryID = Server.HtmlEncode(criteria.CountryID);

                _systemSettingService.InsertCountry(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return AddCountry();
        }

        #endregion Add Country

        #region Delete Country

        /// <summary>
        /// Delete country.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Delete country result.</returns>
        [HttpPost]
        public ActionResult DeleteCountry(CountryDeleteCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.DeleteCountry(criteria);
                var result = GetResultViewModel(criteria.ErrCode, "OK");

                return Json(result);
            }

            return Json(GetResultViewModel(1003));
        }

        #endregion Delete Country

        #endregion Country

        #region Product

        /// <summary>
        /// Show product search.
        /// </summary>
        /// <returns>Product search view.</returns>
        [HttpGet]
        public ActionResult Product()
        {
            return View("Product", new ProductViewModel());
        }

        /// <summary>
        /// Get the list of found products.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>The list of found products.</returns>
        [HttpPost]
        public ActionResult GetProductList(ProductSelectCriteria criteria)
        {
            var data = _systemSettingService.GetProductList(criteria);

            return Json(new
            {
                List = data,
                CurrentPage = 0,
                ItemsPerPage = data.Count(),
                TotalItem = data.Count()
            });
        }

        #region Update Product

        /// <summary>
        /// Show product details.
        /// </summary>
        /// <param name="criteria">Input para.</param>
        /// <returns>Product details view.</returns>
        [HttpGet]
        public ActionResult ProductDetail(ProductDetailCriteria criteria)
        {
            return View("ProductDetail", _systemSettingService.GetProduct(criteria));
        }

        /// <summary>
        /// Update product.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Update product result.</returns>
        [HttpPost]
        public ActionResult UpdateProduct(ProductUpdateCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.UpdateProduct(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return ProductDetail(new ProductDetailCriteria { ID = criteria.ProductID });
        }

        #endregion Update Product

        #region Add Product

        /// <summary>
        /// Show new product UI.
        /// </summary>
        /// <returns>New product UI view.</returns>
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View(new Product());
        }

        /// <summary>
        /// Save new product.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Add product result.</returns>
        [HttpPost]
        public ActionResult AddProduct(ProductAddCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.InsertProduct(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return AddProduct();
        }

        #endregion Add Product

        #region Delete Product

        /// <summary>
        /// Delete product.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Delete product result.</returns>
        [HttpPost]
        public ActionResult DeleteProduct(ProductDeleteCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.DeleteProduct(criteria);
                var result = GetResultViewModel(criteria.ErrCode, "OK");

                return Json(result);
            }

            return Json(GetResultViewModel(1003));
        }

        #endregion Delete Product

        #endregion Product

        #region Language

        /// <summary>
        /// Show language search.
        /// </summary>
        /// <returns>Language search view.</returns>
        [HttpGet]
        public ActionResult Language()
        {
            return View("Language", new LanguageViewModel());
        }

        /// <summary>
        /// Get the list of found languages.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>The list of found languages.</returns>
        [HttpPost]
        public ActionResult GetLanguageList(LanguageSelectCriteria criteria)
        {
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;

            var data = _systemSettingService.GetLanguageList(criteria);

            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        #region Update Language

        /// <summary>
        /// Show language details.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Language details view.</returns>
        [HttpGet]
        public ActionResult LanguageDetail(LanguageDetailCriteria criteria)
        {
            var language = _systemSettingService.GetLanguage(criteria);

            var model = new LanguageUpdateViewModel
                {
                    LanguageID = language.LanguageID,
                    LanguageName = language.LanguageName,
                    CountryID = language.CountryID,
                    CountryList = _systemSettingService.GetCountryList(new CountrySelectCriteria
                        {
                            PageIndex = 1,
                            PageSize = short.MaxValue
                        })
                };

            return View("LanguageDetail", model);
        }

        /// <summary>
        /// Update language.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Update language result.</returns>
        [HttpPost]
        public ActionResult UpdateLanguage(LanguageUpdateCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.UpdateLanguage(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return LanguageDetail(new LanguageDetailCriteria { ID = criteria.LanguageID });
        }

        #endregion Update Language

        #region Add Language

        /// <summary>
        /// Show new language UI.
        /// </summary>
        /// <returns>New language UI view.</returns>
        [HttpGet]
        public ActionResult AddLanguage()
        {
            var model = new LanguageAddViewModel
            {
                CountryList = _systemSettingService.GetCountryList(new CountrySelectCriteria
                    {
                        PageIndex = 1,
                        PageSize = short.MaxValue
                    })
            };

            return View(model);
        }

        /// <summary>
        /// Save new language.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Add language result.</returns>
        [HttpPost]
        public ActionResult AddLanguage(LanguageAddCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.InsertLanguage(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return AddLanguage();
        }

        #endregion Add Language

        #region Delete Language

        /// <summary>
        /// Delete language.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Delete language result.</returns>
        [HttpPost]
        public ActionResult DeleteLanguage(LanguageDeleteCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.DeleteLanguage(criteria);
                var result = GetResultViewModel(criteria.ErrCode, "OK");

                return Json(result);
            }

            return Json(GetResultViewModel(1003));
        }

        #endregion Delete Language

        #endregion Language

        #region Affiliate Type

        /// <summary>
        /// Show affiliate type search.
        /// </summary>
        /// <returns>Affiliate type search view.</returns>
        [HttpGet]
        public ActionResult AffType()
        {
            return View("AffType", new AffTypeViewModel());
        }

        /// <summary>
        /// Get the list of found affiliate types.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>The list of found affiliate types.</returns>
        [HttpPost]
        public ActionResult GetAffTypeList(AffTypeSelectCriteria criteria)
        {
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;

            var data = _systemSettingService.GetAffTypeList(criteria);

            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        #region Update AffType

        /// <summary>
        /// Show affiliate type details.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Affiliate type details view.</returns>
        [HttpGet]
        public ActionResult AffTypeDetail(AffTypeDetailCriteria criteria)
        {
            return View("AffTypeDetail", _systemSettingService.GetAffType(criteria));
        }

        /// <summary>
        /// Update affiliate type.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Update affiliate type result.</returns>
        [HttpPost]
        public ActionResult UpdateAffType(AffTypeUpdateCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.UpdateAffType(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return AffTypeDetail(new AffTypeDetailCriteria { ID = criteria.ID });
        }

        #endregion Update AffType

        #region Add AffType

        /// <summary>
        /// Show new affiliate type UI.
        /// </summary>
        /// <returns>New affiliate type UI view.</returns>
        [HttpGet]
        public ActionResult AddAffType()
        {
            return View(new AffTypeAddViewModel());
        }

        /// <summary>
        /// Save new affiliate type.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Add affiliate type result.</returns>
        [HttpPost]
        public ActionResult AddAffType(AffTypeAddCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.InsertAffType(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return AddAffType();
        }

        #endregion Add AffType

        #region Delete AffType

        /// <summary>
        /// Delete affiliate type.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Delete affiliate type result.</returns>
        [HttpPost]
        public ActionResult DeleteAffType(AffTypeDeleteCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.Updatedby = SessionManager.CurrentSession.CustID;

                _systemSettingService.DeleteAffType(criteria);
                var result = GetResultViewModel(criteria.ErrCode, "OK");

                return Json(result);
            }

            return Json(GetResultViewModel(1003));
        }

        #endregion Delete AffType

        #endregion Affiliate Type
    }
}