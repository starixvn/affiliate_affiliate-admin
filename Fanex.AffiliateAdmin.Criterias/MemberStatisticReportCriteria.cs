// -----------------------------------------------------------------------
// <copyright file="MemberStatisticReportCriteria.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberStatisticReportCriteria : CriteriaBase
    {
        public DateTime ToDate { get; set; }

        public DateTime FromDate { get; set; }

        public string PreferLanguages { get; set; }

        public string Currencies { get; set; }

        public int UserId { get; set; }

        public string AffTypes { get; set; }

        public byte SortBy { get; set; }

        public byte GroupBy { get; set; }

        public bool ExcludeTest { get; set; }

        public string AffName { get; set; }

        public bool IsITAdmin { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_MemberStatisticsReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
