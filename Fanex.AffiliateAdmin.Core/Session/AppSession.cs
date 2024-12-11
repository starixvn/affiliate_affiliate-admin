namespace Fanex.AffiliateAdmin.Core.Session
{
    using Fanex.AffiliateAdmin.Core.Configurations;
    using System;
    using System.Web;

    /// <summary>
    /// App Session.
    /// </summary>
    public static class AppSession
    {
        #region Const
        /// <summary>
        /// Session name for affiliate language.
        /// </summary>
        private const string _LANGUAGE = "AffiliateLanguage";
        #endregion Const

        /// <summary>
        /// Gets or sets Session language.
        /// </summary>
        /// <value>Return Language.</value>
        public static string Language
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session[_LANGUAGE])))
                    {
                        return Convert.ToString(HttpContext.Current.Session[_LANGUAGE]);
                    }
                }

                return AppSettings.DefaultLanguage;
            }

            set
            {
                HttpContext.Current.Session[_LANGUAGE] = value;
            }
        }
    }
}