namespace Fanex.AffiliateAdmin.Criterias.Account
{
    using Fanex.Data.Repository;

    public class AccountUpdateCriteria : CriteriaBase
    {
        public int ModifyBy { get; set; }

        public int UserId { get; set; }

        public int GroupId { get; set; }

        public int LicenseeId { get; set; }

        public string CurrencyIds{ get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_Con_AccountUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}