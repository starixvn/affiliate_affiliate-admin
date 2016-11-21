namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    [Serializable]
    public class AffiliateSession
    {
        public int AffiliateId { get; set; }

        public string Username { get; set; }
        
        public int CurrencyID { get; set; }
    }
}