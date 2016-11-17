namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    [Serializable]
    public class AffiliateSession
    {
        public int AffiliateId { get; set; }

        public string Username { get; set; }
        
        public string Prefix { get; set; }

        public string Currency { get; set; }

        public int CurrencyID { get; set; }

        public string Status { get; set; }

        public string AffTypeName { get; set; }

        public string PrefferredLanguage { get; set; }

        public bool MailOption { get; set; }

        public string Email { get; set; }

        public string PrefferredPayment { get; set; }
    }
}