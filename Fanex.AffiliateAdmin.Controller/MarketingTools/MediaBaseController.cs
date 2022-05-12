namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Services.Interface;
    using Criterias;
    using DTO;
    using Models.MarketingTools;
    using Services;
    using Resources;

    public class MediaBaseController : BaseController
    {
        private readonly ISystemSettings _systemSettings;
        private readonly IMediaBannerService _mediaBannerService;
        private readonly IMediaSettingService _mediaSettingService;

        public MediaBaseController(
            IAppSetting appSetting,
            ISystemSettings systemSettings,
            IMediaBannerService mediaBannerService,
            IMediaSettingService mediaSettingService) : base(appSetting)
        {
            _systemSettings = systemSettings;
            _mediaBannerService = mediaBannerService;
            _mediaSettingService = mediaSettingService;
        }

        protected ISystemSettings SystemSettings => _systemSettings;
        protected IMediaBannerService MediaBannerService => _mediaBannerService;
        protected IMediaSettingService MediaSettingService => _mediaSettingService;

        protected IEnumerable<SelectListItem> GetBannerCategoryOptionList()
        {
            var selectItemList = new List<SelectListItem>();
            var bannerCategories = _mediaSettingService.GetBannerCategories();

            foreach (var item in bannerCategories)
            {
                var value = Lang.ResourceManager.GetString(item.Value.Replace(" ", string.Empty));

                selectItemList.Add(new SelectListItem
                {
                    Text = string.IsNullOrEmpty(value) ? item.Value : value,
                    Value = item.TypeId.ToString()
                });
            }

            selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

            return selectItemList;
        }

        protected IEnumerable<SelectListItem> GetMemberSiteSupportedLanguageOptionList(bool hasAllOption = true)
        {
            var selectItemList = _systemSettings.MemberSiteSiteSupportedLanguages.Select(t => new SelectListItem
            {
                Text = t.LanguageName,
                Value = t.LanguageID
            }).ToList();

            if (hasAllOption)
            {
                selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
            }

            return selectItemList;
        }

        protected IEnumerable<SelectListItem> GetPlayerCurrencyOptionList()
        {
            var selectItemList = _systemSettings.PlayerCurrencies.Select(t => new SelectListItem
            {
                Text = t.CurrencyCode,
                Value = t.CurrencyID.ToString()
            }).ToList();

            if (selectItemList.Count > 1)
            {
                selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
            }

            return selectItemList;
        }

        protected IEnumerable<SelectListItem> GetAffTypeOptionList(short? affType)
        {
            return _systemSettings.AffTypes.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.TypeID.ToString(),
                Selected = t.TypeID == affType
            });
        }

        protected IEnumerable<MediaType> GetBannerTypeList()
        {
            return _mediaSettingService.GetBannerTypes();
        }

        protected IEnumerable<SelectListItem> GetBannerTypeOptionList(short? imageType)
        {
            var bannerTypeList = GetBannerTypeList();

            var selectItemList = bannerTypeList.Select(t => new SelectListItem
            {
                Text = t.Value,
                Value = t.TypeId.ToString(),
                Selected = t.TypeId == imageType
            }).ToList();

            selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

            return selectItemList;
        }

        protected IEnumerable<MediaType> GetBannerSizeList()
        {
            return _mediaSettingService.GetBannerSizes();
        }

        protected IEnumerable<SelectListItem> GetBannerSizeOptionList()
        {
            var bannerSizeList = GetBannerSizeList();

            var selectItemList = bannerSizeList.Select(t => new SelectListItem
            {
                Text = t.Value,
                Value = t.TypeId.ToString()
            }).ToList();

            selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

            return selectItemList;
        }

        protected void AssignMediaListViewModel(TextListViewModel model)
        {
            model.BannerCategoryOptionList = GetBannerCategoryOptionList();
            model.PlayerCurrencyOptionList = GetPlayerCurrencyOptionList();
            model.MemberSiteSupportedLanguageOptionList = GetMemberSiteSupportedLanguageOptionList();
        }

        protected JsonResult GetJsonResult(IEnumerable<Banner> data, BannerListCriteria criteria)
        {
            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }
    }
}
