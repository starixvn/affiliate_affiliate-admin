namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class UserCurrencyInsCriteria : CriteriaBase
    {
        public string CurrencyIds { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public int GroupId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_UserCurrencyIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}