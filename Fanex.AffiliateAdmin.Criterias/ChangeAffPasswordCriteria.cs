namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangeAffPasswordCriteria : CriteriaBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        [SpParam(SqlParamName = "oldpassword")]
        public string Password { get; set; }
        
        [SpParam(SqlParamName = "password")]
        public string ConfirmPassword { get; set; }
        
        public string Ip { get; set; }

        public int ErrCode { get; set; }        

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateUpdPwd";
        }

        public override bool IsValid()
        {
            return Id > 0 && !string.IsNullOrEmpty(Password) 
                && !string.IsNullOrEmpty(ConfirmPassword) 
                && Password.Equals(ConfirmPassword);
        }
    }
}
