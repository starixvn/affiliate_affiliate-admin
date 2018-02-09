namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Models;
    using Resources;
    using Services;

    [FunctionAuth(Function = FunctionPolicyNames.Media)]
    public class MediaTextController : BaseController
    {
        private readonly IMediaBannerService _mediaService;

        public MediaTextController()
            : this(null)
        {
        }

        public MediaTextController(IMediaBannerService mediaService)
        {
            _mediaService = mediaService ?? new MediaBannerService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new TextListViewModel());
        }

        [HttpPost]
        public ActionResult DataList(TextListViewModel model)
        {
            var criteria = new BannerListCriteria
            {
                Category = model.Category,
                Language = model.Language,
                Currency = model.Currency,
                PageIndex = model.PageIndex == 0 ? 1 : model.PageIndex,
                PageSize = model.PageSize == 0 ? AppSettings.PageSize : model.PageSize
            };

            var data = _mediaService.GetTexts(criteria);
            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        [HttpGet]
        public ActionResult AddText()
        {
            var model = new TextAddViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddText(TextAddViewModel model)
        {
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
                    LicenseeId = AppSettings.LicenseeID
                };

                _mediaService.AddBanner(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditText(int id)
        {
            var bannerDetail = _mediaService.GetBannerDetail(new BannerDetailCriteria
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
                    LicenseeId = AppSettings.LicenseeID
                };

                _mediaService.EditBanner(criteria);
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

                _mediaService.DeleteBanner(criteria);

                return ModelResult(criteria.ErrCode);
            }

            return InvalidModelResult();
        }
    }
}