namespace Nexcel.Affiliate.Integration
{
    using System;
    using System.IO;
    using System.Web;
    using Nexcel.Affiliate.Integration.Configurations;
    using System.Security.Cryptography;
    public class UrlFactory : IUrlFactory
    {
        private const string EncryptedParamPattern = "lic={0}&user={1}&pwd={2}&time={3}&lang={4}";
        private const string Pass = "2conthanhlanconduanhaucanchoi";
        private IIntegrationSettings _settings;

        public UrlFactory()
            : this(null)
        {
        }

        public UrlFactory(IIntegrationSettings settings)
        {
            this._settings = settings ?? new IntegrationSettings();
        }

        public string GenerateSsoLink(IRequestSsoModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (!model.IsValid)
            {
                throw new ArgumentException(Resources.InvalidRequestModel);
            }

            string publicKeyPath = this._settings.PublicKeyPath;

            if (!File.Exists(publicKeyPath))
            {
                throw new FileNotFoundException(publicKeyPath);
            }

            string inputString = string.Format(
                EncryptedParamPattern,
                this._settings.LicenseeName,
                model.Username,
                model.EncryptedPassword,
                model.Timestamp,
                model.LanguageCode);


            var encryptionThread = new Crypt();
            string str = encryptionThread.Encrypt(inputString, Pass);
            return this._settings.SsoUrl + "?key=" + str;
        }
    }
}