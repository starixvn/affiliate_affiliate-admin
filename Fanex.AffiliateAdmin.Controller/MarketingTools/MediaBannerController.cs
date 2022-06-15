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
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;
    using Models.MarketingTools;
    using Resources;

    [FunctionAuth(Function = FunctionPolicyNames.Media)]
    public class MediaBannerController : MediaBaseController
    {
        private int CustID => SessionManager.CurrentSession.CustID;

        public MediaBannerController(
            IAppSetting appSetting,
            ISystemSettings systemSettings,
            IMediaBannerService mediaBannerService,
            IMediaSettingService mediaSettingService) : base(appSetting, systemSettings, mediaBannerService, mediaSettingService)
        {
        }

        [HttpGet]
        public ActionResult Index(BannerListViewModel model)
        {
            AssignMediaListViewModel(model);

            model.BannerSizeOptionList = GetBannerSizeOptionList();
            model.AffiliateTypeOptionList = GetAffTypeOptionList(model.AffType);
            model.BannerTypeOptionList = GetBannerTypeOptionList(model.ImageType);

            return View(model);
        }

        [HttpPost]
        public ActionResult DataList(BannerListCriteria criteria)
        {
            AssignPagingCriteriaBase(criteria);
            var data = MediaBannerService.GetBanners(criteria);

            foreach (var banner in data)
            {
                var content = banner.Content.Replace('/', '\\');
                var filename = Path.GetFileNameWithoutExtension(content);
                var extension = Path.GetExtension(content);
                var thumbnailFile = string.Format(BannerStorage.ThumbNailFormat, filename, extension);

                banner.Thumbnail = Path.Combine(Path.GetDirectoryName(content), thumbnailFile).Replace('\\', '/');
            }

            return GetJsonResult(data, criteria);
        }

        [HttpGet]
        public ActionResult AddBanner()
        {
            var categories = MediaSettingService.GetBannerCategories();
            var selectedCategory = categories.First().TypeId;

            var model = new BannerAddViewModel
            {
                Category = selectedCategory, // must pass this value to get sub categories
                AffType = SystemSettings.AffTypes.First().TypeID
            };

            AssignBannerViewModel(model, categories);

            return View(model);
        }

        [HttpPost]
        public ActionResult AddBanner(BannerAddViewModel model)
        {
            AssignBannerViewModel(model);

            if (ModelState.IsValid)
            {
                var bannerStorage = BannerHelper.UploadFile(model, out string errorMessage);

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
                        UserId = CustID,
                        Ip = Remote.GetClientIPAddress(Request),
                        Language = model.Language,
                        LicenseeId = SystemSettings.LicenseeID
                    };

                    MediaBannerService.AddBanner(criteria);

                    if (criteria.ErrCode != 0)
                    {
                        // delete banner if failed
                        bannerStorage.DeleteBanners();
                    }

                    ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditBanner(int id)
        {
            var bannerDetail = MediaBannerService.GetBannerDetail(new BannerDetailCriteria
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
                }
                else
                {
                    model.Category = (short)(bannerDetail.Category != null ? bannerDetail.Category.Value : 0);
                }

                AssignBannerViewModel(model);

                return View("EditBanner", model);
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditBanner(BannerEditViewModel model)
        {
            AssignBannerViewModel(model);

            if (ModelState.IsValid)
            {
                var bannerDetail = MediaBannerService.GetBannerDetail(new BannerDetailCriteria { BannerId = model.BannerId });
                var bannerStorage = BannerHelper.UpdateFile(model, bannerDetail, GetBannerSizeList(),  out string errorMessage);

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
                        UserId = CustID,
                        Ip = Remote.GetClientIPAddress(Request),
                        LicenseeId = SystemSettings.LicenseeID
                    };

                    MediaBannerService.EditBanner(criteria);

                    if (criteria.ErrCode != 0)
                    {
                        bannerStorage.DeleteBanners();
                    }

                    ViewBag.Result = GetResultViewModel(criteria.ErrCode);
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
                    UserId = CustID,
                    Ip = Remote.GetClientIPAddress(Request)
                };

                MediaBannerService.DeleteBanner(criteria);

                // delete banner files
                var contents = model.Contents.Split(',');
                foreach (var content in contents)
                {
                    BannerHelper.DeleteFile(content);
                }

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
                SubCategories = MediaSettingService.GetBannerSubCategories(category)
            };

            return PartialView(model);
        }

        private IEnumerable<SelectListItem> GetBannerCommonOptionList(IEnumerable<MediaType> mediaTypes, short? typeId)
        {
            var bannerOptionList = new List<SelectListItem>();
            foreach (var media in mediaTypes)
            {
                var text = Lang.ResourceManager.GetString(media.Value.Replace(" ", string.Empty));

                bannerOptionList.Add(new SelectListItem()
                {
                    Value = media.TypeId.ToString(),
                    Text = string.IsNullOrEmpty(text) ? media.Value : text,
                    Selected = typeId == media.TypeId
                });
            }

            return bannerOptionList;
        }

        private void AssignBannerViewModel(BannerInfoViewModel model, IEnumerable<MediaType> categories = null)
        {
            var bannerTypes = MediaSettingService.GetBannerTypes();
            categories = categories ?? MediaSettingService.GetBannerCategories();

            model.SubCategories = model.SubCategories ?? MediaSettingService.GetBannerSubCategories(model.Category);

            model.AffiliateTypeOptionList = SelectListHelpers.GetAffiliateTypeOptionList(SystemSettings.AffTypes, model.AffType.ToString());
            model.PlayerCurrencyOptionList = SelectListHelpers.GetPlayerCurrencyOptionList(SystemSettings.PlayerCurrencies, model.Currency.ToString());
            model.BannerTypeOptionList = GetBannerCommonOptionList(bannerTypes, model.ImageType);
            model.BannerCategoryOptionList = GetBannerCommonOptionList(categories, model.Category);
            model.BannerSubCategoryOptionList = GetBannerCommonOptionList(model.SubCategories, model.SubCategory);
            model.MemberSiteSupportedLanguageOptionList = SelectListHelpers.GetLanguageOptionList(SystemSettings.MemberSiteSiteSupportedLanguages, model.Language);
        }
    }
}