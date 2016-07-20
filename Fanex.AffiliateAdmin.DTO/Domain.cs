namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Domain class.
    /// </summary>
    public class Domain
    {
        public int DomainID { get; set; }

        public string DomainName { get; set; }

        public string Creator { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateDateStr
        {
            get
            {
                return CreateDate.HasValue ? CreateDate.Value.ToString(DateFormatManager.DateFormat) : string.Empty;
            }
        }
    }
}
