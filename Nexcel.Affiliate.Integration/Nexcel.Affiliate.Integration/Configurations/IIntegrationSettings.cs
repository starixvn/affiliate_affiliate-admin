using System;
using System.Collections.Generic;
using System.Text;

namespace Nexcel.Affiliate.Integration.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIntegrationSettings
    {
        string PublicKeyPath
        {
            get;
            set;
        }
        string SsoUrl
        {
            get;
            set;
        }
        string InternalLoginUrl
        {
            get;
            set;
        }
        string LicenseeName
        {
            get;
            set;
        }
    }
}
