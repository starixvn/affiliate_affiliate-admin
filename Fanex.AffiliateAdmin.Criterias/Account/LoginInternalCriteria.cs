namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Login Criteria.
    /// </summary>
    public class LoginInternalCriteria : CriteriaBase
    {
        public string LicenseeName { get; set; }

        public string Username { get; set; }

        [SpParam(SqlParamName = "EncryptedPassword")]
        public string Password { get; set; }

        public DateTime LoginTime { get; set; }

        public string LoginIP { get; set; }

        public string LoginDomain { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string Language { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int ServerId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_InternalLogin";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
