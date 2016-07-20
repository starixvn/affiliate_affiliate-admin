namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Login Criteria.
    /// </summary>
    public class LoginCriteria : CriteriaBase
    {
        public string Username { get; set; }

        [SpParam(Ignored = true)]
        public string UserPwd { get; set; }

        public string LoginIP { get; set; }

        public string LoginDomain { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string Langkey { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int ServerId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_UserLogin";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
