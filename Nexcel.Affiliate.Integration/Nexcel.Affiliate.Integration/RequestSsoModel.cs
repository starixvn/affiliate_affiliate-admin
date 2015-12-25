namespace Nexcel.Affiliate.Integration
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class RequestSsoModel : IRequestSsoModel
    {
        public RequestSsoModel()
        {
            this.LanguageCode = "en-US";
        }

        public string Username { get; set; }

        public string EncryptedPassword { get; set; }

        public long Timestamp { get; set; }

        public bool IsValid
        {
            get
            {
                return
                    !string.IsNullOrEmpty(this.Username)
                    && !string.IsNullOrEmpty(this.EncryptedPassword) && this.Timestamp > 0L
                    && !string.IsNullOrEmpty(this.LanguageCode);
            }
        }

        /// <summary>
        /// Default Value= en-US
        /// The language Value is the national language code
        /// example:en-US,zh-TW,zh-CN
        /// for more language code ,plz go to:http://msdn.microsoft.com/en-us/library/ms533052(v=vs.85).aspx 
        /// Required:true
        /// </summary>
        public string LanguageCode { get; set; }
    }
}
