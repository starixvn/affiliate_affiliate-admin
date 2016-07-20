namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberDetail
    {
        public string UserName { get; set; }

        public string AffUserName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? RegisteredDate { get; set; }

        public string Currency { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public DateTime? AffAffectedDate { get; set; }

        public bool IsRemoved { get; set; }

        public DateTime? RemovedDate { get; set; }

        public string RemovedBy { get; set; }
    }
}