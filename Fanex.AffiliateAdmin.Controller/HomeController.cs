namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Services;

    [NoCache]
    public class HomeController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IHomeService _homeService;

        public HomeController(
            IAppSetting appSetting,
            IHomeService homeService,
            ISystemSettingService systemSettingService) : base(systemSettingService)
        {
            _appSetting = appSetting;
            _homeService = homeService;
        }

        public ActionResult Index()
        {
            if (SubMenu.Home.Count() == 1)
            {
                return new RedirectResult("~/Home/UserOnlineList");
            }

            FormaterCurrency.SetDefaultCurrency(_appSetting.BaseExchange.Code);
            var model = _homeService.GeneralInfoSel(new HomeGeneralInfoCriteria
            {
                UserCurrencies = SessionManager.CurrentSession.BuildUserCurrencyParam()
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
            var languageList = _appSetting.BaseLanguages;
            var selectedLang = languageList.FirstOrDefault(lang => lang.LanguageID == AppSession.Language);
            var languageName = selectedLang == null ? string.Empty : selectedLang.LanguageName;
            var sysDateTime = _homeService.GetSystemDateTime(new SystemDateTimeCriteria());
            return PartialView(Tuple.Create(languageList, languageName, sysDateTime));
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
            var session = SessionManager.CurrentSession;

            criteria.IsIT = session.IsIT;
            criteria.LicenseeId = _appSetting.LicenseeID;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();

            var listUseronlines = _homeService.UserOnlineList(criteria);

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