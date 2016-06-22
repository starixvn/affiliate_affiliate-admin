namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffType
    {
        public byte TypeID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "NameFieldIsRequired")]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.HasValue ? CreatedDate.Value.ToString(DateFormatManager.DateFormatFull) : string.Empty;
            }
        }

        public string CreatorName { get; set; }
    }
}