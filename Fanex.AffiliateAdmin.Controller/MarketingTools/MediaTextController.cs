namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Core.Filter;
    using Core.Services.Interface;
    using Models.MarketingTools;
    using Resources;
    using Services;

    [FunctionAuth(Function = FunctionPolicyNames.Media)]
    public class MediaTextController : MediaBaseController
    {
        public MediaTextController(
            IAppSetting appSetting,
            ISystemSettings systemSettings,
            IMediaBannerService mediaBannerService,
            IMediaSettingService mediaSettingService) : base(appSetting, systemSettings, mediaBannerService, mediaSettingService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new TextListViewModel();
            AssignMediaListViewModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult DataList(BannerListCriteria criteria)
        {
            AssignBaseCriteria(criteria);
            var data = MediaBannerService.GetTexts(criteria);

            return GetJsonResult(data, criteria);
        }

        [HttpGet]
        public ActionResult AddText()
        {
            var model = new TextAddViewModel();
            AssignTextViewModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult AddText(TextAddViewModel model)
        {
            AssignTextViewModel(model);
            if (ModelState.IsValid)
            {
                var criteria = new BannerInsCriteria
                {
                    UserId = SessionManager.CurrentSession.CustID,
                    Ip = Remote.GetClientIPAddress(),
                    Content = HttpUtility.HtmlEncode(model.Content),
                    Category = model.Category,
                    Currency = model.Currency,
                    Language = model.Language,
                    AffType = 0, // banner text
                    LicenseeId = SystemSettings.LicenseeID
                };

                MediaBannerService.AddBanner(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditText(int id)
        {
            var bannerDetail = MediaBannerService.GetBannerDetail(new BannerDetailCriteria
            {
                BannerId = id
            });

            var model = new TextEditViewModel
            {
                Content = HttpUtility.HtmlDecode(bannerDetail.Content),
                Category = bannerDetail.GetCategory,
                Language = bannerDetail.Language,
                Currency = bannerDetail.Currency.Value,
                BannerId = id
            };
            AssignTextViewModel(model);

            var selectedCurrency = model.PlayerCurrencyOptionList.FirstOrDefault(t => t.Value == model.Currency.ToString());
            if (selectedCurrency != null)
            {
                selectedCurrency.Selected = true;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditText(TextEditViewModel model)
        {
            AssignTextViewModel(model);
            if (ModelState.IsValid)
            {
                var criteria = new BannerUpdateCriteria
                {
                    BannerId = model.BannerId,
                    Category = model.Category,
                    Content = HttpUtility.HtmlEncode(model.Content),
                    Language = model.Language,
                    Currency = model.Currency,
                    UserId = SessionManager.CurrentSession.CustID,
                    Ip = Remote.GetClientIPAddress(),
                    LicenseeId = SystemSettings.LicenseeID
                };

                MediaBannerService.EditBanner(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteText(BannerDeleteCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.Ip = Remote.GetClientIPAddress();

                MediaBannerService.DeleteBanner(criteria);

                return ModelResult(criteria.ErrCode);
            }

            return InvalidModelResult();
        }

        private void AssignTextViewModel(TextAddViewModel model)
        {
            model.BannerCategoryOptionList = GetBannerCategoryOptionList();
            model.PlayerCurrencyOptionList = GetPlayerCurrencyOptionList();
            model.MemberSiteSupportedLanguageOptionList = GetMemberSiteSupportedLanguageOptionList(false);
        }
    }
}