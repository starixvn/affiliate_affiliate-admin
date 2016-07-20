namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// User Online class.
    /// </summary>
    public class UserOnline
    {
        public string Username { get; set; }
        
        public DateTime LoggedIn { get; set; }

        public string LoggedInStr
        {
            get
            {
                return LoggedIn.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public DateTime LastRequest { get; set; }

        public string LastRequestStr
        {
            get
            {
                return LastRequest.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public string Ip { get; set; }

        public string Role { get; set; }
    }
}