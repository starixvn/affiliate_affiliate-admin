// --------------------------------------------------------------------------
// <copyright file="Customer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.8.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Customer class.
    /// </summary>
    public class Customer
    {
        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public string Currency { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public bool IsRemoved { get; set; }

        public DateTime? RemovedDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime RegisteredDate { get; set; }

        public int AffID { get; set; }

        public string AffUserName { get; set; }

        public string AffTypeName { get; set; }
    }
}