namespace Fanex.AffiliateAdmin.Core.Session
{
    using System.Runtime.Serialization;
    using System.Web;
    using Logging;

    /// <summary>
    /// Session Manager.
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// Affiliate Current Session Key.
        /// </summary>
        private const string _AffiliateCurrentSessionKey = "Fanex.AffiliateAdmin.Core.CurrentSession";

        public static ICurrentSession CurrentSession
        {
            get
            {
                try
                {
                    return (ICurrentSession)HttpContext.Current.Session[_AffiliateCurrentSessionKey];
                }
                catch (SerializationException exception)
                {
                    LogService.Info("Session Error: " + exception.Message);
                }

                return null;
            }

            set
            {
                HttpContext.Current.Session[_AffiliateCurrentSessionKey] = value;
            }
        }

        /// <summary>
        /// Clear All Session.
        /// </summary>
        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }
    }
}