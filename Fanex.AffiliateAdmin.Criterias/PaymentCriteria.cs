namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string AffType { get; set; }

        public string Status { get; set; }

        public string PreferredLanguage { get; set; }

        public string Currency { get; set; }

        public short DateOption { get; set; }

        public DateTime? Fromdate { get; set; }

        public DateTime? Todate { get; set; }

        public byte BaseCurrency { get; set; }

        public bool IsITAdmin { get; set; }

        public bool ExcludeTest { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PaymentRequestSelList";
        }

        public override bool IsValid()
        {
            if ((DateOption > 0 && (Fromdate == null || Todate == null)) || Currency == null)
            {
                return false;
            }

            return true;
        }
    }
}