using System;
using System.Collections.Generic;
using System.Text;
using Nexcel.Affiliate.Integration.Configurations;
using System.IO;
using RSACryptoPad;
using System.Text.RegularExpressions;

namespace Nexcel.Affiliate.Integration
{
    public class UrlFactory : IUrlFactory
    {
        private const string EncryptedParamPattern = "licenseename={0}&username={1}&password={2}&timestamp={3}&loginurl={4}&language={5}";
        private readonly Regex UrlRegularExpression = new Regex("(http|https):\\/\\/([\\w@][\\w.:@]+)\\/?[\\w\\.?=%&=\\-@/$,]*");

        private IIntegrationSettings _settings;
        public UrlFactory()
            : this(null)
        {
        }

        public UrlFactory(IIntegrationSettings settings)
        {
            this._settings = (settings ?? new IntegrationSettings());
        }

        public string GenerateSsoLink(IRequestSsoModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (!model.IsValid)
            {
                throw new Exception(Resources.InvalidRequestModel);
            }

            string publicKeyPath = this._settings.PublicKeyPath;
            if (!File.Exists(publicKeyPath))
            {
                throw new FileNotFoundException(publicKeyPath);
            }
            if (!string.IsNullOrEmpty(model.ActiveDomain) && !UrlRegularExpression.IsMatch(model.ActiveDomain))
            {
                throw new Exception(string.Format(Resources.InvalidUrlFormat, "SsoUrl"));
            }
            string inputString = string.Format(EncryptedParamPattern, new object[]
			{
				this._settings.LicenseeName,
				model.Username,
				model.EncryptedPassword,
				model.Timestamp,
				this._settings.InternalLoginUrl,
				model.LanguageCode
			});
            EncryptionThread encryptionThread = new EncryptionThread();
            string str = encryptionThread.EncryptString(inputString, 1024, publicKeyPath);
            return string.IsNullOrEmpty(model.ActiveDomain) ? this._settings.SsoUrl + "?key=" + str
                : model.ActiveDomain + "?key=" + str;
        }
    }
}
