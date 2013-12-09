// --------------------------------------------------------------------------
// <copyright file="AffiliateInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Affiliate Information.
    /// </summary>
    public class AffiliateInfo
    {
        public long? RowNo { get; set; }

        public int? AffID { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public bool Favorite { get; set; }

        public string LastName { get; set; }

        public string LanguageName { get; set; }

        public string AffTypeName { get; set; }

        public DateTime? RegisteredDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string Status { get; set; }

        public byte TestLevel { get; set; }

        public string AccountType { get; set; }

        public int? CommGroupID { get; set; }

        public string CommGroupName { get; set; }

        public string Currency { get; set; }

        public int CurrencyId { get; set; }

        public decimal? BF { get; set; }

        public int? TotalMembers { get; set; }
    }
}