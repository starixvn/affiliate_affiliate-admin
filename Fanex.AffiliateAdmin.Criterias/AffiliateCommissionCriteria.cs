// --------------------------------------------------------------------------
// <copyright file="AffiliateCommissionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.3">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Commission group criteria class.
    /// </summary>
    public class AffiliateCommissionCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public string Currencies { get; set; }

        public byte BaseCurrency { get; set; }

        public byte SortBy { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte TransferStatus { get; set; }

        public byte GroupBy { get; set; }

        public int PageCurrent { get; set; }

        public int PageSize { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateCommissionReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}