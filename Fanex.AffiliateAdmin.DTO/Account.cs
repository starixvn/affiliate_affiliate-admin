// -----------------------------------------------------------------------
// <copyright file="Account.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Muller.Nguyen" date="2013.8.20">Add properties</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    public class Account
    {
        public int CustID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string UserSalt { get; set; }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public bool IsIT { get; set; }

        public int OnUserID { get; set; }

        public int ServerId { get; set; }
    }
}
