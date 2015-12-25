// -----------------------------------------------------------------------
// <copyright file="AffiliateUpdCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateUpdCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrFName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrLName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrGender")]
        public bool Gender { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrCountry")]
        public string Country { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrEmail")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$",
            ErrorMessageResourceType = typeof(Lang),
            ErrorMessageResourceName = "ErrEmailValid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Password { get; set; }

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

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrChequePostcode")]
        public string Postal { get; set; }

        [SpParam(SqlParamName = "emailformat")]
        public bool MailOption { get; set; }

        public string Currency { get; set; }

        public string PlayerCurrency { get; set; }

        public bool IsPageEnabled { get; set; }

        public int Level { get; set; }

        [SpParam(SqlParamName = "SiteUrl")]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrSiteUrl")]
        public string SiteLink { get; set; }

        public string MoreSiteUrls { get; set; }

        public string PreferredLanguage { get; set; }

        public string PreferredPayment { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ErrUserNamePlayer")]
        [SpParam(SqlParamName = "player_username")]
        public string PlayerUserName { get; set; }

        [SpParam(SqlParamName = "neteller_accountid")]
        public string NetellerAccountId { get; set; }

        [SpParam(SqlParamName = "moneybookers_email")]
        public string MoneyBookersEmail { get; set; }

        [SpParam(SqlParamName = "wire_bankname")]
        public string WireBankName { get; set; }

        [SpParam(SqlParamName = "wire_bankaddress")]
        public string WireBankAddress { get; set; }

        [SpParam(SqlParamName = "wire_swiftcode")]
        public string WireSwiftCode { get; set; }

        [SpParam(SqlParamName = "wire_accountname")]
        public string WireAccountName { get; set; }

        [SpParam(SqlParamName = "wire_accountno")]
        public string WireAccountNo { get; set; }

        [SpParam(SqlParamName = "cheque_fullname")]
        public string ChequeFullName { get; set; }

        [SpParam(SqlParamName = "cheque_address")]
        public string ChequeAddress { get; set; }

        [SpParam(SqlParamName = "cheque_city")]
        public string ChequeCity { get; set; }

        [SpParam(SqlParamName = "cheque_state")]
        public string ChequeState { get; set; }

        [SpParam(SqlParamName = "cheque_postal")]
        public string ChequePostal { get; set; }

        [SpParam(SqlParamName = "cheque_country")]
        public string ChequeCountry { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateUpdInfo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}