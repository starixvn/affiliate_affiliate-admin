// -----------------------------------------------------------------------
// <copyright file="AffiliateSession.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class AffiliateSession
    {
        public int AffiliateId { get; set; }

        public string UserName { get; set; }

        public string Prefix { get; set; }

        public string Currency { get; set; }

        public int CurrencyID { get; set; }

        public string Status { get; set; }

        public int AffType { get; set; }

        public int GroupID { get; set; }

        public string PrefferredLanguage { get; set; }

        public bool MailOption { get; set; }

        public string Email { get; set; }

        public string PrefferredPayment { get; set; }
    }
}
