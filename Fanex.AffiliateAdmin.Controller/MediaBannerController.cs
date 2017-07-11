namespace Fanex.AffiliateAdmin.Controller
{
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;
    using Models;
    using Resources;

    [FunctionAuth(Function = FunctionPolicyNames.Media)]
    public class MediaBannerController : BaseController
    {
        private readonly IMediaBannerService _mediaService;

        private readonly IMediaSettingService _mediaSettingService;

        public MediaBannerController()
            : this(null, null)
        {
        }

        public MediaBannerController(IMediaBannerService mediaService, IMediaSettingService mediaSettingService)
        {
            _mediaService = mediaService ?? new MediaBannerService();
            _mediaSettingService = mediaSettingService ?? new MediaSettingService();
        }

        [HttpGet]
        public ActionResult Index(BannerListViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult DataList(BannerListCriteria criteria)
        {
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;
            var data = _mediaService.GetBanners(criteria);

            foreach (var banner in data)
            {
                var content = banner.Content.Replace('/', '\\');
                var filename = Path.GetFileNameWithoutExtension(content);
                var extension = Path.GetExtension(content);
                var thumbnailFile = string.Format(BannerStorage.ThumbNailFormat, filename, extension);

                banner.Thumbnail = Path.Combine(Path.GetDirectoryName(content), thumbnailFile).Replace('\\', '/');
            }

            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        [HttpGet]
        public ActionResult AddBanner()
        {
            var firstCategory = MediaSettings.BannerCategories.First().TypeId;

            var model = new BannerAddViewModel
            {
                Category = firstCategory, // must pass this value to get sub categories
                SubCategories = _mediaSettingService.GetBannerSubCategories(firstCategory),
                AffType = AppSettings.AffTypes.First().TypeID
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddBanner(BannerAddViewModel model)
        {
            model.SubCategories = _mediaSettingService.GetBannerSubCategories(model.Category);

            if (ModelState.IsValid)
            {
                string errorMessage;
                var bannerStorage = BannerHelper.UploadFile(model, out errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
                else
                {
                    var criteria = new BannerInsCriteria
                    {
                        Category = model.GetCategory,
                        Currency = model.Currency,
                        AffType = model.AffType,
                        ImageType = model.ImageType,
                        Content = bannerStorage.RelativeBannerPath.Replace('\\', '/'),
                        Size = bannerStorage.BannerSize,
                        UserId = SessionManager.CurrentSession.CustID,
                        Ip = Remote.GetClientIPAddress(),
                        Language = model.Language,
                        LicenseeId = AppSettings.LicenseeID
                    };

                    _mediaService.AddBanner(criteria);

                    if (criteria.ErrCode != 0)
                    {
                        // delete banner if failed
                        bannerStorage.DeleteBanners();
                    }

                    ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);

                    MediaSettings.RefreshBannerSizes();
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditBanner(int id)
        {
            var bannerDetail = _mediaService.GetBannerDetail(new BannerDetailCriteria
            {
                BannerId = id
            });

            if (bannerDetail.Type != null && bannerDetail.Currency != null && bannerDetail.ImageType != null)
            {
                var model = new BannerEditViewModel
                {
                    BannerId = bannerDetail.BannerID,
                    AffType = bannerDetail.Type.Value,
                    Language = bannerDetail.Language,
                    Currency = bannerDetail.Currency.Value,
                    ImageType = bannerDetail.ImageType.Value,
                    FileName = Path.GetFileName(bannerDetail.Content),
                };

                if (bannerDetail.ParentID != null)
                {
                    model.Category = bannerDetail.ParentID.Value;
                    model.SubCategory = bannerDetail.Category;
                    model.SubCategories = _mediaSettingService.GetBannerSubCategories(model.Category);
                }
                else
                {
                    model.Category = (short)(bannerDetail.Category != null ? bannerDetail.Category.Value : 0);
                    model.SubCategories = _mediaSettingService.GetBannerSubCategories(model.Category);
                }

                return View("EditBanner", model);
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditBanner(BannerEditViewModel model)
        {
            model.SubCategories = _mediaSettingService.GetBannerSubCategories(model.Category);

            if (ModelState.IsValid)
            {
                string errorMessage;
                var bannerDetail = _mediaService.GetBannerDetail(new BannerDetailCriteria { BannerId = model.BannerId });
                var bannerStorage = BannerHelper.UpdateFile(model, bannerDetail, out errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
                else
                {
                    var criteria = new BannerUpdateCriteria
                    {
                        BannerId = bannerDetail.BannerID,
                        Category = model.GetCategory,
                        Currency = model.Currency,
                        Language = model.Language,
                        AffType = model.AffType,
                        ImageType = model.ImageType,
                        Content = bannerStorage.RelativeBannerPath.Replace('\\', '/'),
                        Size = bannerStorage.BannerSize,
                        UserId = SessionManager.CurrentSession.CustID,
                        Ip = Remote.GetClientIPAddress(),
                        LicenseeId = AppSettings.LicenseeID
                    };

                    _mediaService.EditBanner(criteria);

                    if (criteria.ErrCode != 0)
                    {
                        bannerStorage.DeleteBanners();
                    }

                    ViewBag.Result = GetResultViewModel(criteria.ErrCode);

                    MediaSettings.RefreshBannerSizes();
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteBanner(BannerDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new BannerDeleteCriteria
                {
                    BannerIds = model.BannerIds,
                    UserId = SessionManager.CurrentSession.CustID,
                    Ip = Remote.GetClientIPAddress(),
                    LicenseeId = AppSettings.LicenseeID
                };

                _mediaService.DeleteBanner(criteria);

                // delete banner files
                var contents = model.Contents.Split(',');
                foreach (var content in contents)
                {
                    BannerHelper.DeleteFile(content);
                }

                MediaSettings.RefreshBannerSizes();

                return ModelResult(criteria.ErrCode);
            }

            return InvalidModelResult();
        }

        [HttpPost]
        public ActionResult SubCategories(short category, bool isShowAll)
        {
            var model = new SubCategoriesViewModel
            {
                IsShowAll = isShowAll,
                SubCategories = _mediaSettingService.GetBannerSubCategories(category)
            };

            return PartialView(model);
        }
    }
}