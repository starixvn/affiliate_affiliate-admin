// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class User
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string UserSalt { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Status { get; set; }

        public bool IsInternal { get; set; }
    }
}
