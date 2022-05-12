namespace Fanex.AffiliateAdmin.Core.Session
{
    using System;
    using System.Web;
    using Fanex.AffiliateAdmin.Core.Configurations;

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

                return AppSetting.Instance.DefaultLanguage;
            }

            set
            {
                HttpContext.Current.Session[_LANGUAGE] = value;
            }
        }
    }
}