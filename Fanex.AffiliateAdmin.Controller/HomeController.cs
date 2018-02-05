namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Services;

    [NoCache]
    public class HomeController : BaseController
    {
        private const string HomeMenuName = "Home";

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
            if (SubMenu.Home.Count() == 1)
            {
                return new RedirectResult("~/Home/UserOnlineList");
            }

            FormaterCurrency.SetDefaultCurrency(AppSettings.BaseExchange.Code);
            var model = _homeService.GeneralInfoSel(new HomeGeneralInfoCriteria
            {
                UserId = SessionManager.CurrentSession.CustID,
                LicenseeId = AppSettings.LicenseeID,
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
            var languageList = AppSettings.BaseLanguages;
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

            criteria.UserId = session.CustID;
            criteria.IsIT = session.IsIT;
            criteria.LicenseeId = AppSettings.LicenseeID;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();
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