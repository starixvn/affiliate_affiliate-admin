// --------------------------------------------------------------------------
// <copyright file="MediaController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Services;
    using Models;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[Media]")]
    public class MediaController : BaseController
    {
        #region Fields

        private readonly IMediaService _mediaService;

        private readonly IMediaSettingService _mediaSettingService;

        #endregion

        #region Constructors

        public MediaController(IMediaService mediaService, IMediaSettingService mediaSettingService)
        {
            _mediaService = mediaService;
            _mediaSettingService = mediaSettingService;
        }

        #endregion

       /// <summary>
       /// Banner List.
       /// </summary>
       /// <param name="model">Input model.</param>
       /// <returns>Banner list page.</returns>
        [HttpGet]
        public ActionResult Index(BannerListViewModel model)
        {
            return View("BannerList", model);
        }

        #region Text List

        /// <summary>
        /// Text list.
        /// </summary>
        /// <returns>Html page of text list.</returns>
        [HttpGet]
        public ActionResult TextList()
        {
            return View(new TextListViewModel());
        }

        /// <summary>
        /// Text list data.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns a list of text.</returns>
        [HttpPost]
        public ActionResult TextDataList(TextListViewModel model)
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

        /// <summary>
        /// Adds the banner text.
        /// </summary>
        /// <returns>Page html.</returns>
        [HttpGet]
        public ActionResult AddText()
        {
            var model = new TextAddViewModel();
            return View(model);
        }

        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Return result.</returns>
        public ActionResult AddText(TextAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new BannerInsCriteria
                {
                    UserId = CurrentSession.CustID,
                    Ip = Remote.GetClientIPAddress(),
                    Content = HttpUtility.HtmlEncode(model.Content),
                    Category = model.Category,
                    Currency = model.Currency,
                    Language = model.Language,
                    AffType = 0 // banner text
                };

                _mediaService.AddBanner(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return View(model);
        }

        /// <summary>
        /// Action Get EditText.
        /// </summary>
        /// <param name="id">Text Id value.</param>
        /// <returns>Action result.</returns>
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

            return View(model);
        }

        /// <summary>
        /// Action POST EditText.
        /// </summary>
        /// <param name="model">Text Edit View Model.</param>
        /// <returns>Action result.</returns>
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
                    UserId = CurrentSession.CustID,
                    Ip = Remote.GetClientIPAddress()
                };

                _mediaService.EditBanner(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return View(model);
        }

        /// <summary>
        /// Action POST DeleteText.
        /// </summary>
        /// <param name="criteria">BannerDeleteCriteria object.</param>
        /// <returns>Action result.</returns>
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

        #endregion Text List

        #region Banner List

        /// <summary>
        /// Get banner list page.
        /// </summary>
        /// <param name="model">The model object.</param>
        /// <returns>Return banner list.</returns>
        [HttpGet]
        public ActionResult BannerList(BannerListViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Get banner data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return banner list data.</returns>
        [HttpPost]
        public ActionResult BannerDataList(BannerListCriteria criteria)
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
                string bannerPath, thumbnailPath;
                GetBannerPath(banner.Content, out bannerPath, out thumbnailPath);

                if (System.IO.File.Exists(thumbnailPath))
                {
                    banner.Thumbnail = Path.Combine(Path.GetDirectoryName(content), thumbnailFile);
                }
            }

            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        /// <summary>
        /// Adds the banner.
        /// </summary>
        /// <returns>Page html.</returns>
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

        /// <summary>
        /// Adds the banner.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Return result.</returns>
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
                            UserId = CurrentSession.CustID,
                            Ip = Remote.GetClientIPAddress()
                        };

                    _mediaService.AddBanner(criteria);

                    if (criteria.ErrCode != 0)
                    {
                        // delete banner if failed
                        bannerStorage.DeleteBanner();
                    }

                    ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);

                    MediaSettings.RefreshBannerSizes();
                }
            }

            return View(model);
        }

        /// <summary>
        /// Edit banner.
        /// </summary>
        /// <param name="id">Banner id.</param>
        /// <returns>Edit banner view.</returns>
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

        /// <summary>
        /// Edit banner.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Edit banner view.</returns>
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
                            UserId = CurrentSession.CustID,
                            Ip = Remote.GetClientIPAddress()
                        };

                    _mediaService.EditBanner(criteria);

                    if (criteria.ErrCode != 0)
                    {
                        bannerStorage.DeleteBanner();
                    }

                    ViewBag.Result = GetResultViewModel(criteria.ErrCode);

                    MediaSettings.RefreshBannerSizes();
                }
            }

            return View(model);
        }

        /// <summary>
        /// Delete banner.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Delete banner view.</returns>
        [HttpPost]
        public ActionResult DeleteBanner(BannerDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new BannerDeleteCriteria
                {
                    BannerIds = model.BannerIds,
                    UserId = SessionManager.CurrentSession.CustID,
                    Ip = Remote.GetClientIPAddress()
                };

                _mediaService.DeleteBanner(criteria);

                // delete banner files
                var contents = model.Contents.Split(',');
                foreach (var content in contents)
                {
                    DeleteBannerFile(content);
                }

                return ModelResult(criteria.ErrCode);
            }

            return InvalidModelResult();
        }

        /// <summary>
        /// Render sub category list by category.
        /// </summary>
        /// <param name="category">Category Id.</param>
        /// <param name="isShowAll">Has/ don't have All item in sub category.</param>
        /// <returns>Sub category view.</returns>
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

        /// <summary>
        /// Delete the banner/thumbnail file.
        /// </summary>
        /// <param name="content">The relative banner path.</param>
        private void DeleteBannerFile(string content)
        {
            string bannerPath, thumbnailPath;
            GetBannerPath(content, out bannerPath, out thumbnailPath);

            Common.DeleteFile(bannerPath);
            Common.DeleteFile(thumbnailPath);
        }

        /// <summary>
        /// Upload new banner file.
        /// </summary>
        /// <param name="model">The model object.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The uploading result.</returns>
        private BannerStorage UploadBannerFile(BannerAddViewModel model, out string errorMessage)
        {
            var bannerStorage = new BannerStorage
            {
                AffType = model.AffType,
                Category = model.Category,
                SubCategory = model.SubCategory,
                BannerFile = model.FileName,
                File = model.File,
                BannerType = model.ImageType
            };

            errorMessage = bannerStorage.UploadBanner();

            return bannerStorage;
        }

        /// <summary>
        /// Update banner file.
        /// </summary>
        /// <param name="model">The model object.</param>
        /// <param name="bannerDetail">The banner detail.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The updating result.</returns>
        private BannerStorage UpdateBannerFile(BannerEditViewModel model, BannerDetail bannerDetail, out string errorMessage)
        {
            errorMessage = string.Empty;
            var bannerStorage = new BannerStorage
            {
                AffType = model.AffType,
                Category = model.Category,
                SubCategory = model.SubCategory,
                BannerFile = model.FileName,
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

                    // Update the position of banner file.
                    // Determine the old banner/thumbnail path.       
                    string oldBannerPath, oldThumbnailPath;
                    GetBannerPath(bannerDetail.Content, out oldBannerPath, out oldThumbnailPath);

                    if (bannerDetail.ImageType.HasValue)
                    {
                        errorMessage = bannerStorage.MoveBannerFile(oldBannerPath, oldThumbnailPath);
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

        /// <summary>
        /// Get banner path, thumbnail path.
        /// </summary>
        /// <param name="content">The relative banner path.</param>
        /// <param name="bannerPath">The banner path.</param>
        /// <param name="thumbnailPath">The thumbnail path.</param>
        private void GetBannerPath(string content, out string bannerPath, out string thumbnailPath)
        {
            var oldContent = content.Replace('/', '\\');
            var oldBannerFile = Path.GetFileName(oldContent);
            var oldThumbnailFile = string.Format(
                BannerStorage.ThumbNailFormat,
                Path.GetFileNameWithoutExtension(oldContent),
                Path.GetExtension(oldContent));

            bannerPath = Path.Combine(MediaSettings.BannerDirectory, oldContent);
            thumbnailPath = bannerPath.Replace(oldBannerFile, oldThumbnailFile);
        }

        #endregion
    }
}