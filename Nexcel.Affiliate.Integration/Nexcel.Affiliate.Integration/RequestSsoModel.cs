using System;
using System.Collections.Generic;
using System.Text;

namespace Nexcel.Affiliate.Integration
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestSsoModel : IRequestSsoModel
    {
        public RequestSsoModel()
        {
            this.LanguageCode = "en-US";
        }
        private string _langaugeCode;
        private string _username;
        private string _encryptedPassword;
        private long _timeStamp;
        /// <summary>
        /// Required:true
        /// </summary>
        public string Username
        {
            get
            {
                return this._username;
            }
            set
            {
                this._username = value;
            }
        }
        /// <summary>
        /// Required:true
        /// </summary>
        public string EncryptedPassword
        {
            get
            {
                return this._encryptedPassword;
            }
            set
            {
                this._encryptedPassword = value;
            }
        }
        /// <summary>
        /// Required:true
        /// </summary>
        public long Timestamp
        {
            get
            {
                return this._timeStamp;
            }
            set
            {
                this._timeStamp = value;
            }
        }
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
        public string LanguageCode
        {
            get { return _langaugeCode; }
            set { this._langaugeCode = value; }
        }
        /// <summary>
        /// if ActiveDomain is null or Empty,the system will use domain in webcongif file
        /// </summary>
        public string ActiveDomain { get; set; }
    }
}
