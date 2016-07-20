namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Customer class.
    /// </summary>
    public class Customer
    {
        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public string Currency { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public bool IsRemoved { get; set; }

        public DateTime? RemovedDate { get; set; }

        public string RemovedDateStr
        {
            get
            {
                return RemovedDate.HasValue ? RemovedDate.Value.ToString(DateFormatManager.DateFormat) : string.Empty;
            }
        }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.HasValue ? CreatedDate.Value.ToString(DateFormatManager.DateFormat) : string.Empty;
            }
        }

        public DateTime? RegisteredDate { get; set; }

        public string RegisteredDateStr
        {
            get
            {
                return RegisteredDate.HasValue ? RegisteredDate.Value.ToString(DateFormatManager.DateFormat) : string.Empty;
            }
        }

        public DateTime? AffAffectedDate { get; set; }

        public string AffAffectedDateStr
        {
            get
            {
                return AffAffectedDate.HasValue ? AffAffectedDate.Value.ToString(DateFormatManager.DateFormat) : string.Empty;
            }
        }

        public int AffID { get; set; }

        public string AffUserName { get; set; }

        public string AffTypeName { get; set; }
    }
}