namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SystemSettingUpdateViewModel
    {
        public int SId { get; set; }

[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Allow Type Property")]
        public string Type { get; set; }

        public string SName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ValueFieldIsRequired")]
        public string SValue { get; set; }

        public string Description { get; set; }

        public string Environment { get; set; }

        public IEnumerable<SystemSetting> EnvironmentList { get; set; }

        public IEnumerable<SelectListItem> EnvironmentOptionList
        {
            get 
            {
                var selectItemList = EnvironmentList.Select(t => new SelectListItem
                { 
                    Text = t.SValue,
                    Value = t.SValue,
                    Selected = !string.IsNullOrEmpty(Environment) && t.SValue.Equals(Environment)
                }).ToList();

                selectItemList.Insert(0, new SelectListItem { Text = Lang.None, Value = string.Empty });

                return selectItemList;
            }
        }
    }
}