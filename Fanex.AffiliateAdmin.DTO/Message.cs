namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Message
    {
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Accep en abbreviation")]
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Accept contain underscores")]
        public string Message_en { get; set; }

        public int Creator { get; set; }
        
        public DateTime PostedDate { get; set; }

        public string PostedDateStr
        {
            get
            {
                return PostedDate.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public int Msgid { get; set; }

        public bool Important { get; set; }

        public string CreatorName { get; set; }

        public string UpdateName { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedDateStr
        {
            get
            {
                return UpdatedDate.HasValue ? UpdatedDate.Value.ToString(DateFormatManager.DateFormatFull) : string.Empty;
            }
        }
    }
}
