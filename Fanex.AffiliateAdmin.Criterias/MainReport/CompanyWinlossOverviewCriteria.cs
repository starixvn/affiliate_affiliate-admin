// --------------------------------------------------------------------------
// <copyright file="CompanyWinlossOverviewCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Company Win loss Report Criteria.
    /// </summary>
    public class CompanyWinlossOverviewCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public string PlayerCurrencies { get; set; }

        public byte GroupBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte DrawFilter { get; set; }

        public byte BaseCurrency { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        [SpParam(Ignored = true)]
        public bool ByAffiliate { get; set; }

        public int PageSize { get; set; }

        [SpParam(SqlParamName = "PageCurrent")]
        public int PageIndex { get; set; }

        public int NRows { get; set; }

        public int Duration { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CompanyWinlossReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}