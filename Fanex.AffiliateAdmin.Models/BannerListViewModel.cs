// --------------------------------------------------------------------------
// <copyright file="BannerListViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerListViewModel : TextListViewModel
    {
        #region Fields

        private IEnumerable<SelectListItem> _bannerTypeList;

        private IEnumerable<SelectListItem> _bannerSizeList;

        #endregion

        [Display(Name = "ImageType", ResourceType = typeof(Lang))]
        public short? ImageType { get; set; }

        [Display(Name = "Size", ResourceType = typeof(Lang))]
        public string Size { get; set; }

        [Display(Name = "AffType", ResourceType = typeof(Lang))]
        public short? AffType { get; set; }

        #region Option List

        public IEnumerable<SelectListItem> AffiliateTypeOptionList
        {
            get
            {
                var selectItemList = AppSettings.AffTypes.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.TypeID.ToString(),
                    Selected = AffType.HasValue ? t.TypeID == AffType.Value : false
                }).ToList();

                return selectItemList;
            }
        }

        public IEnumerable<SelectListItem> BannerTypeOptionList
        {
            get
            {
                if (_bannerTypeList == null)
                {
                    var selectItemList = MediaSettings.BannerTypes.Select(t => new SelectListItem
                    {
                        Text = t.Value,
                        Value = t.TypeId.ToString(),
                        Selected = ImageType.HasValue ? t.TypeId == ImageType.Value : false
                    }).ToList();

                    selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
                    _bannerTypeList = selectItemList;
                }

                return _bannerTypeList;
            }
        }

        public IEnumerable<SelectListItem> BannerSizeOptionList
        {
            get
            {
                if (_bannerSizeList == null)
                {
                    var selectItemList = MediaSettings.BannerSizes.Select(t => new SelectListItem
                    {
                        Text = t.Value,
                        Value = t.TypeId.ToString()
                    }).ToList();

                    selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
                    _bannerSizeList = selectItemList;
                }

                return _bannerSizeList;
            }
        }

        #endregion
    }
}