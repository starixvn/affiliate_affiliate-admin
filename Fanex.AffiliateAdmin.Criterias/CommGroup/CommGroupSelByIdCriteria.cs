namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommGroupSelByIdCriteria : CriteriaBase
    {
        public int GroupID { get; set; }

        public int UserId { get; set; }

        [SpParam(Ignored = true)]
        public DateTime? Date { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupSelByID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
