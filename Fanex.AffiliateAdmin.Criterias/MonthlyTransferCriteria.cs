namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyTransferCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public override string GetSettingKey()
        {
            return "Task_Aff_MonthlyTransfer";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}