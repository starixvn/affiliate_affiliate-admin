// -----------------------------------------------------------------------
// <copyright file="ExternalLogin.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Security;
    using Fanex.AffiliateAdmin.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ExternalLogin
    {
        private const int PASSWORDLENGTH = 6;

        /// <summary>
        /// Get string url external login.
        /// </summary>
        /// <param name="loginInfo">External Login Info.</param>
        /// <returns>Return URL link.</returns>
        public static Uri ExternalLoginUrl(ExternalLoginInfo loginInfo)
        {
            var chalengeStr = Generator.GetRandomString(PASSWORDLENGTH);

            const string LINKFORMAT = "{0}/User/AdminLogin?chalengeStr={1}&accKey={2}&custName={3}&accId={4}&accOnId={5}";

            var accKey = HashPassword(loginInfo.SessionID + chalengeStr);

            var siteInfo = SiteDomainInfo.Parse(loginInfo.Controller, loginInfo.IsKiosk());

            return new Uri(string.Format(LINKFORMAT, siteInfo.AffiliateSiteUrl, chalengeStr, accKey, loginInfo.Username, loginInfo.CustId, loginInfo.OnUserID));
        }

        private static string HashPassword(string value)
        {
            MD5 algorithm = MD5.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            string result = "";
            for (int i = 0; i < data.Length; i++)
            {
                result += data[i].ToString("x2").ToUpperInvariant();
            }
            return result;
        }
    }
}
