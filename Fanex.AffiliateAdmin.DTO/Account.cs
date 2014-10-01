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
    using System;

    /// <summary>
    /// Admin Account.
    /// </summary>
    public class Account
    {
        public int CustID { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Status { get; set; }

        public string Creator { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateStr
        {
            get
            {
                return CreateDate.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public bool IsIT { get; set; }

        public int OnUserID { get; set; }
    }
}
