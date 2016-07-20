namespace Fanex.AffiliateAdmin.DTO
{
    using System.ComponentModel.DataAnnotations;
    using Resources;
    
    /// <summary>
    /// Country class.
    /// </summary>
    public class Country
    {
        [StringLength(6, ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ExceedCharacters")]
        [RegularExpression(@"^([a-zA-Z-]+)$", ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "InvalidID")]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "IDFieldIsRequired")]
        public string CountryID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "NameFieldIsRequired")]
        [Display(ResourceType = typeof(Lang), Name = "Name")]
        public string CountryName { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "DisplayOrderFieldIsRequired")]
        [Range(0, short.MaxValue, ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "DisplayOrderRange")]
        public short DisplayOrder { get; set; }

        public bool IsEnabled { get; set; }
    }
}