// --------------------------------------------------------------------------
// <copyright file="PlayerWinlossReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.11.6">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Player Win loss Report Criteria.
    /// </summary>
    public class PlayerWinlossReportCriteria : CriteriaBase
    {
        public bool ByAffiliate { get; set; }

        public int UserId { get; set; }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public string PlayerName { get; set; }

        public byte GroupBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string PlayerCurrencies { get; set; }

        public byte BaseCurrency { get; set; }

        public byte DrawFilter { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PlayerWinlossReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}