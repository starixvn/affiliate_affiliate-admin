// -----------------------------------------------------------------------
// <copyright file="AffiliateUpdCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateUpdCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Gender { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Postal { get; set; }

        public string SiteUrl { get; set; }

        public string MoreSiteUrls { get; set; }

        public string PreferredPayment { get; set; }

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

        [SpParam(SqlParamName = "emailformat")]
        public bool MailOption { get; set; }

        public string PlayerCurrency { get; set; }

        public string Currency { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

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
