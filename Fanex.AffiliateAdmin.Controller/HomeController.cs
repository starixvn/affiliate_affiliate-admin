// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.8.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Filter;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Services.Interface;

    /// <summary>
    /// Home Controller class.
    /// </summary>
    [NoCache]
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public ActionResult Index()
        {
            var result = _homeService.GeneralInfoSel(new HomeGeneralInfoCriteria
            {
                UserId = SessionManager.CurrentSession.CustID
            });
            var model = result.FirstOrDefault();
            FormaterCurrency.SetDefaultCurrency(AppSettings.BaseExchange.Code);

            return View(model);
        }

        public ActionResult Home()
        {
            var result = _homeService.GeneralInfoSel(new HomeGeneralInfoCriteria());
            var model = result.FirstOrDefault();

            return View(model);
        }

        /// <summary>
        /// Load language list to select box.
        /// </summary>
        /// <returns>Language list.</returns>
        [ChildActionOnly]
        public ActionResult Language()
        {
            var languageList = AppSettings.BaseLanguages;
            var selectedLang = languageList.Where(lang => lang.LanguageID == AppSession.Language).FirstOrDefault().LanguageName;
            var sysDateTime = _homeService.GetSystemDateTime(new SystemDateTimeCriteria());
            return PartialView(Tuple.Create(languageList, selectedLang, sysDateTime));
        }

        /// <summary>
        /// Change language.
        /// </summary>
        /// <param name="lang">Set language.</param>
        /// <returns>Change language of page.</returns>
        [HttpPost]
        public ActionResult ChangeLanguage(string lang)
        {
            AppSession.Language = lang;
            return Json(string.Empty);
        }

        /// <summary>
        /// Menu of page.
        /// </summary>
        /// <returns>Main menu of page.</returns>
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult UserOnlineList()
        {
            return View();
        }

        /// <summary>
        /// User Online List.
        /// </summary>
        /// <param name="criteria">Home Online List Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult UserOnlineList(HomeOnlineListCriteria criteria)
        {
            criteria.IsIT = SessionManager.CurrentSession.IsIT;
            var listUseronlines = _homeService.UserOnlineList(criteria).ToList();
            return Json(new
            {
                List = listUseronlines,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }
    }
}