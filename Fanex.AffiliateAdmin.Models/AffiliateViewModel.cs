// -----------------------------------------------------------------------
// <copyright file="AffiliateViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateViewModel
    {
        public int AffId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PreferredLanguage { get; set; }

        public bool IsClosed { get; set; }

        public string GroupName { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string Currency { get; set; }

        public int NoOfMember { get; set; }

        public decimal BalanceBF { get; set; }

        public DateTime ApproveDate { get; set; }

        public int Favorite { get; set; }

        public int GroupId { get; set; }

        public string Prefix { get; set; }

        public string Status { get; set; }

        public bool IsLocked { get; set; }

        public string AffTypeName { get; set; }

        public bool IsAllowedPersonalizedPage { get; set; }

        public bool HasSecurityQuestion { get; set; }

        public bool MailOption { get; set; }

        public int AffLevel { get; set; }

        public int AffType { get; set; }

        public bool Gender { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Postcode { get; set; }

        public bool EmailFormat { get; set; }

        public Uri SiteURL { get; set; }

        public string MoreSiteURLs { get; set; }

        public string PreferredPayment { get; set; }

        public string Player_Username { get; set; }

        public string PlayerCurrency { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Ip { get; set; }

        public string Browser { get; set; }

        public string LanguageName { get; set; }

        public int OnUserID { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string LastLoginIp { get; set; }
    }
}
