// --------------------------------------------------------------------------
// <copyright file="MemberListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.14">Created</change>
//   <change who="Brian.Dinh" date="2013.11.27">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Country Criteria.
    /// </summary>
    public class MemberListCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        /// <value>Id of current Admin.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets Affiliate Username.
        /// </summary>
        /// <value>Affiliate Username.</value>
        public string AffUsername { get; set; }

        /// <summary>
        /// Gets or sets Username.
        /// </summary>
        /// <value>Username of Customer.</value>
        public string Username { get; set; }
        
        public string AffType { get; set; }

        public string CurrencyList { get; set; }

        public byte MemberType { get; set; }

        public byte DateOption { get; set; }

        public DateTime? FromDate { get; set; }
        
        public DateTime? ToDate { get; set; }

        public byte SortBy { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        public bool ByAffiliate { get; set; }
        
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_MemberListSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}