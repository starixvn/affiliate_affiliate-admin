namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Deduction Setting class.
    /// </summary>
    public class DeductionSetting
    {
        public byte DeductionID { get; set; }

        public byte ProductID { get; set; }

        public int GroupID { get; set; }

        public decimal DeductedPercentage { get; set; }
    }
}