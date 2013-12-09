// -----------------------------------------------------------------------
// <copyright file="ExternalLogin.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Security;
    using Fanex.AffiliateAdmin.Core.Configurations;
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExternalLogin
    {
        /// <summary>
        /// Get random string.
        /// </summary>
        /// <param name="length">Length of string random.</param>
        /// <returns>Result of string after random.</returns>
        public static string GetRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        /// <summary>
        /// Get string url external login.
        /// </summary>
        /// <param name="custName">Customer name.</param>
        /// <param name="custId">Customer ID.</param>
        /// <param name="userId">On user ID of affiliate.</param>
        /// <param name="sessionId">Application session ID.</param>
        /// <param name="affType">Affiliate type.</param>
        /// <returns>Link external.</returns>
        public static string ExternalLoginUrl(string custName, int custId, int userId, string sessionId, int affType)
        {
            var chalengeStr = ExternalLogin.GetRandomString(6);

            var accKey = FormsAuthentication.HashPasswordForStoringInConfigFile(sessionId + chalengeStr, "MD5");

            if (affType == 3)
            {
                return string.Format(AppSettings.InternalURLKiosk, chalengeStr, accKey, custName, custId, userId);
            }

            return string.Format(AppSettings.InternalURL, chalengeStr, accKey, custName, custId, userId);
        }
    }
}
