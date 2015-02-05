namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// User Currency Insert Criteria.
    /// </summary>
    public class UserCurrencyInsCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string CurrencyXML { get; set; }

        public int ModifyBy { get; set; }

        public string Langkey { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_UserCurrencyIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}