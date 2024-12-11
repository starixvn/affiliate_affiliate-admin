namespace Fanex.AffiliateAdmin.Models
{
    using DTO;
    using Resources;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ConfigurationViewModel
    {
        public SystemSetting TypeList { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public IEnumerable<SelectListItem> TypeOptionList
        {
            get
            {
                var selectItemList = TypeList.SValue.Split(',').Select(t => new SelectListItem
                {
                    Text = t,
                    Value = t
                }).ToList();

                selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

                return selectItemList;
            }
        }
    }
}