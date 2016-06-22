namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LanguageUpdateViewModel
    {
        public string LanguageID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "NameFieldIsRequired")]
        public string LanguageName { get; set; }

        public string CountryID { get; set; }

        public IEnumerable<DTO.Country> CountryList { get; set; }

        public IEnumerable<SelectListItem> CountryOptionList
        {
            get
            {
                var selectItemList = CountryList.Select(t => new SelectListItem
                {
                    Value = t.CountryID,
                    Text = t.CountryID == "-" ? Resources.Country.Others : Resources.Country.ResourceManager.GetString(t.CountryID),
                    Selected = t.CountryID.Equals(CountryID)
                }).ToList();

                return selectItemList;
            }
        }
    }
}