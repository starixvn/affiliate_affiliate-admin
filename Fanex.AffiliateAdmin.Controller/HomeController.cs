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
    using System.Web.SessionState;
    using Core.Configurations;
    using Core.Filter;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Home Controller class.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    [NoCache]
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;

        public HomeController()
            : this(null)
        {
        }

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService ?? new HomeService();
        }

        public ActionResult Index()
        {
            FormaterCurrency.SetDefaultCurrency(AppSettings.BaseExchange.Code);
            var model = _homeService.GeneralInfoSel(new HomeGeneralInfoCriteria
            {
                UserId = SessionManager.CurrentSession.CustID
            });

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
            var selectedLang = languageList.FirstOrDefault(lang => lang.LanguageID == AppSession.Language).LanguageName;
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