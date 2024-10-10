namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class AccountPasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }
        
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public int LicenseeId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_AdminUpdPwd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
