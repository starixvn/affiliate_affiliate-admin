// -----------------------------------------------------------------------
// <copyright file="QualifiedMembersReportCriteria.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class QualifiedMembersReportCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int AffId { get; set; }

        public byte BaseCurrency { get; set; }

        [SpParam(Ignored = true)]
        public string Currency { get; set; }

        public DateTime MonthDate { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_QualifiedMembersReport";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Products);
        }
    }
}
