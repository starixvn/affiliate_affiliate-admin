// --------------------------------------------------------------------------
// <copyright file="SubCategoriesViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SubCategoriesViewModel
    {
        public IEnumerable<MediaType> SubCategories { get; set; }

        public IEnumerable<SelectListItem> SubCategoryOptionList
        {
            get
            {
                var selectItemList = SubCategories.Select(t => new SelectListItem
                {
                    Text = t.Value,
                    Value = t.TypeId.ToString()
                }).ToList();

                if (IsShowAll)
                {
                    selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });
                }

                return selectItemList;
            }
        }

        public bool IsShowAll { get; set; }
    }
}
