// --------------------------------------------------------------------------
// <copyright file="Affiliate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// Affiliate class.
    /// </summary>
    public class Affiliate
    {
        public int AffId { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrFName")]
        [Display(Name = "FirstName", ResourceType = typeof(Lang))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrLName")]
        [Display(Name = "LastName", ResourceType = typeof(Lang))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrEmail")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", 
            ErrorMessageResourceType = typeof(Lang), 
            ErrorMessageResourceName = "ErrEmailValid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("PreferredLanguage")]
        public string PreferredLanguage { get; set; }

        public bool IsClosed { get; set; }

        public string GroupName { get; set; }

        public DateTime RegisteredDate { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public short Currency { get; set; }

        [Display(Name = "CurrencyCode", ResourceType = typeof(Lang))]
        public string CurrencyCode { get; set; }

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

        public byte AffType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrGender")]
        public bool Gender { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrPhone")]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrMobilePhone")]
        public string MobilePhone { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrAddress")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrCity")]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrState")]
        public string State { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrCountry")]
        [Display(Name = "Country", ResourceType = typeof(Lang))]
        public string CountryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrChequePostcode")]
        public string Postal { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrSiteUrl")]
        public string SiteLink { get; set; }

        public string MoreSiteURLs { get; set; }

        public string PreferredPayment { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrUserNamePlayer")]
        [DisplayName("PlayerUsername")]
        public string Player_Username { get; set; }

        [Display(Name = "MemberCurrency", ResourceType = typeof(Lang))]
        public string PlayerCurrency { get; set; }

        public string Question { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrAnwser")]
        public string Answer { get; set; }

        public string Ip { get; set; }

        public string Browser { get; set; }

        public string LanguageName { get; set; }

        public int OnUserID { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string LastLoginIp { get; set; }
    }
}