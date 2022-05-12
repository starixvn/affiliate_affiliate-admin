namespace Fanex.AffiliateAdmin.Models.MarketingTools
{
    using System.Collections.Generic;
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
                var selectItemList = new List<SelectListItem>();
                foreach (var item in SubCategories)
                {
                    var value = Lang.ResourceManager.GetString(item.Value.Replace(" ", string.Empty));
                    selectItemList.Add(new SelectListItem
                    {
                        Text = string.IsNullOrEmpty(value) ? item.Value : value,
                        Value = item.TypeId.ToString()
                    });
                }

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
