namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Services;
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

        #region Banner List

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
                var bannerStorage = UploadBannerFile(model, out errorMessage);

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
                var bannerStorage = UpdateBannerFile(model, bannerDetail, out errorMessage);

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
                    DeleteBannerFile(content);
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

        #endregion Banner List

        #region Banner Helpers

        private void DeleteBannerFile(string content)
        {
            IEnumerable<string> bannerPaths, thumbnailPaths;
            GetBannerPaths(content, out bannerPaths, out thumbnailPaths);

            Common.DeleteFiles(bannerPaths);
            Common.DeleteFiles(thumbnailPaths);
        }

        private BannerStorage UploadBannerFile(BannerAddViewModel model, out string errorMessage)
        {
            var bannerStorage = new BannerStorage
            {
                AffType = model.AffType,
                Category = model.Category,
                SubCategory = model.SubCategory,
                BannerFileName = model.FileName,
                File = model.File,
                BannerType = model.ImageType
            };

            errorMessage = bannerStorage.UploadBanner();

            return bannerStorage;
        }

        private BannerStorage UpdateBannerFile(BannerEditViewModel model, BannerDetail bannerDetail, out string errorMessage)
        {
            errorMessage = string.Empty;
            var bannerStorage = new BannerStorage
            {
                AffType = model.AffType,
                Category = model.Category,
                SubCategory = model.SubCategory,
                BannerFileName = model.FileName,
                File = model.File,
                BannerType = model.ImageType
            };

            if (bannerStorage.File == null)
            {
                // Check the valid extension.
                if (!bannerStorage.IsValidExtension())
                {
                    errorMessage = Lang.errFileExtension;
                }
                else
                {
                    var mediaType = MediaSettings.BannerSizes.FirstOrDefault(t => t.TypeId == bannerDetail.Size);
                    if (mediaType != null)
                    {
                        bannerStorage.BannerSize = mediaType.Value;
                    }

                    if (bannerDetail.ImageType.HasValue)
                    {
                        errorMessage = bannerStorage.MoveBannerFile(bannerDetail.Content);
                    }
                }
            }
            else
            {
                errorMessage = bannerStorage.UploadBanner();
                if (string.IsNullOrEmpty(errorMessage))
                {
                    // Delete the old banner file.
                    DeleteBannerFile(bannerDetail.Content);
                }
            }

            return bannerStorage;
        }

        private void GetBannerPaths(
            string content,
            out IEnumerable<string> bannerPaths,
            out IEnumerable<string> thumbnailPaths)
        {
            var oldContent = content.Replace('/', '\\');

            var oldBannerFile = Path.GetFileName(oldContent);
            var oldThumbnailFile = BannerStorage.GetThumbnailFileName(oldContent);

            bannerPaths = MediaSettings.BannerFolders.Select(x => Path.Combine(x, oldContent));
            thumbnailPaths = bannerPaths.Select(x => x.Replace(oldBannerFile, oldThumbnailFile));
        }

        #endregion Banner Helpers
    }
}