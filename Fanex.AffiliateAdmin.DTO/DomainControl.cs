namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Domain Control class.
    /// </summary>
    public class DomainControl
    {
        public int ID { get; set; }

        public int AffDomainId { get; set; }

        public string AffDomain { get; set; }

        public int MemberDomainId { get; set; }

        public string MemberDomain { get; set; }

        public string CountryId { get; set; }

        public string CountryName { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateStr
        {
            get
            {
                return CreateDate.ToString(DateFormatManager.DateFormat);
            }
        }

        public DateTime ModifyDate { get; set; }

        public string ModifyDateStr
        {
            get
            {
                return ModifyDate.ToString(DateFormatManager.DateFormat);
            }
        }

        public string Creator { get; set; }

        public bool Status { get; set; }
    }
}
