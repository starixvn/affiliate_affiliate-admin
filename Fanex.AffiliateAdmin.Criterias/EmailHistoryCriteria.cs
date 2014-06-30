// --------------------------------------------------------------------------
// <copyright file="EmailHistoryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Email History Criteria.
    /// </summary>
    public class EmailHistoryCriteria : CriteriaBase
    {
        public CriteriaType CriteriaType { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        public int? MID { get; set; }

        public string SenderName { get; set; }

        public string Email { get; set; }

        public string AffiliateUserName { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        public string ListMID { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            switch (this.CriteriaType)
            {
                case CriteriaType.Delete:
                    return "AffAdmin_EmailHistoryDel";
                default:
                    return "AffAdmin_EmailHistorySel";
            }            
        }

        public override bool IsValid()
        {
            return PageSize > 0 && PageIndex > 0;
        }
    }
}