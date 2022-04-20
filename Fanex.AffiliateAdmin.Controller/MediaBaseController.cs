namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Services.Interface;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;
    using Models;
    using Resources;

    public class MediaBaseController : BaseController
    {
        private readonly IAppSetting _appSetting;
        private readonly IMediaBannerService _mediaBannerService;
        private readonly IMediaSettingService _mediaSettingService;

        public MediaBaseController(
            IAppSetting appSetting,
            IMediaBannerService mediaBannerService,
            IMediaSettingService mediaSettingService,
            ISystemSettingService systemSettingService) : base(systemSettingService)
        {
            _appSetting = appSetting;
            _mediaBannerService = mediaBannerService;
            _mediaSettingService = mediaSettingService;
        }

        protected IAppSetting AppSetting => _appSetting;
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

        protected IEnumerable<Language> GetMemberSiteSupportedLanguageList()
        {
            return SystemSettingService.GetMemberSiteSupportedLanguageList(
                            new MemberSiteLanguageCriteria
                            {
                                Environment = _appSetting.Environment
                            });
        }

        protected IEnumerable<SelectListItem> GetMemberSiteSupportedLanguageOptionList(bool hasAllOption = true)
        {
            var languageList = GetMemberSiteSupportedLanguageList();

            var selectItemList = languageList.Select(t => new SelectListItem
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

        protected IEnumerable<Currency> GetPlayerCurrencyList()
        {
            return SystemSettingService.GetPlayerCurrencyList(new PlayerCurrencyCriteria());
        }

        protected IEnumerable<SelectListItem> GetPlayerCurrencyOptionList()
        {
            var playerCurrencies = GetPlayerCurrencyList();
            var selectItemList = playerCurrencies.Select(t => new SelectListItem
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

        protected IEnumerable<AffType> GetAffTypeList()
        {
            return SystemSettingService.GetAffTypeList(new AffTypeCriteria());
        }

        protected IEnumerable<SelectListItem> GetAffTypeOptionList(short? affType)
        {
            var affTypeList = GetAffTypeList();

            return affTypeList.Select(t => new SelectListItem
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

        protected void AssignBannerListCriteria(BannerListCriteria criteria)
        {
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSetting.PageSize : criteria.PageSize;
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
