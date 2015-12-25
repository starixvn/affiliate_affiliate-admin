namespace Nexcel.Affiliate.Integration.Configurations
{
    using System;

    public interface IIntegrationSettings
    {
        string PublicKeyPath
        {
            get;
            set;
        }

        Uri SsoUrl
        {
            get;
            set;
        }

        Uri InternalLoginUrl
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
