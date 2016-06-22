namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExternalLoginInfo
    {
        public Controller Controller { get; set; }

        public string Username { get; set; }

        public int CustId { get; set; }

        public int OnUserID { get; set; }

        public string SessionID { get; set; }

        public string AffTypeName { get; set; }

        /// <summary>
        /// Check whether it belong to kiosk type or not.
        /// </summary>
        /// <returns>Return true or false.</returns>
        public bool IsKiosk()
        {
            return AffTypeName.Equals("kiosk", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
