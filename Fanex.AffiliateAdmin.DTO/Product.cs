namespace Fanex.AffiliateAdmin.DTO
{
    using Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Product class.
    /// </summary>
    public class Product
    {
        public byte ProductID { get; set; }

        [StringLength(12, ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ExceedCharacters")]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "NameFieldIsRequired")]
        public string ProductName { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        [Range(0, Int32.MaxValue, ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "APIProductIDRange")]
        public int? API_ProductID { get; set; }

        public int? Creator { get; set; }

        public string CreatorName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.HasValue ? CreatedDate.Value.ToString(DateFormatManager.DateFormatFull) : string.Empty;
            }
        }

        public string Description { get; set; }

        public decimal? QMTurnOver { get; set; }

        public decimal? QMDeposit { get; set; }

        public string ReportColumns { get; set; }

        public string BettypeList { get; set; }

        public int TotalQualifiedMembers { get; set; }

        public bool IsEnabled { get; set; }
    }
}