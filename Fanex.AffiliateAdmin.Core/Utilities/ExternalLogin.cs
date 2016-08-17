// -----------------------------------------------------------------------
// <copyright file="ExternalLogin.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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

#pragma warning disable 0618 // Type or member is obsolete
            var accKey = FormsAuthentication.HashPasswordForStoringInConfigFile(loginInfo.SessionID + chalengeStr, "MD5");
#pragma warning restore 0618 // Type or member is obsolete

            var siteInfo = SiteDomainInfo.Parse(loginInfo.Controller, loginInfo.IsKiosk());

            return new Uri(string.Format(LINKFORMAT, siteInfo.AffiliateSiteUrl, chalengeStr, accKey, loginInfo.Username, loginInfo.CustId, loginInfo.OnUserID));
        }
    }
}