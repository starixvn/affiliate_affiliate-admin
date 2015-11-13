namespace Nexcel.Affiliate.Integration
{
    using System;

    public interface IRequestSsoModel
    {
        string Username
        {
            get;
            set;
        }

        string EncryptedPassword
        {
            get;
            set;
        }

        long Timestamp
        {
            get;
            set;
        }

        bool IsValid
        {
            get;
        }

        /// <summary>
        /// Default Value= en-US
        /// The language Value is the national language code
        /// example:en-US,zh-TW,zh-CN
        /// for more language code ,plz go to:http://msdn.microsoft.com/en-us/library/ms533052(v=vs.85).aspx 
        /// </summary>
        string LanguageCode { get; set; }

        Uri ActiveDomain { get; set; }
    }
}
