namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentUpdateCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public string Status { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        [SpParam(SqlParamName = "AdminDesc")]
        public string AdminRemark { get; set; }

        [SpParam(SqlParamName = "AffDesc")]
        public string AffRemark { get; set; }

        public long RefNo { get; set; }

        public long IbcRefNo { get; set; }

        public long IbcTransId { get; set; }

        public string IbcUsername { get; set; }

        public string PreferredLanguage { get; set; }

        public bool MailOption { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PaymentRequestUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}