// --------------------------------------------------------------------------
// <copyright file="AffiliateSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.6.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class AffiliateSession
    {
        public int AffiliateId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string Prefix { get; set; }

        public string Currency { get; set; }

        public int CurrencyID { get; set; }

        public string Status { get; set; }

        public int AffType { get; set; }

        public string AffTypeName { get; set; }

        public int GroupID { get; set; }

        public string PrefferredLanguage { get; set; }

        public bool MailOption { get; set; }

        public string Email { get; set; }

        public string PrefferredPayment { get; set; }
    }
}
