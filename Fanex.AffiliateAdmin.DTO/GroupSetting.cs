namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupSetting
    {
        public int GroupID { get; set; }

        public byte ProductID { get; set; }

        public byte TierID { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }

        public int MinQM { get; set; }

        public int MaxQM { get; set; }

        public decimal Commission { get; set; }
    }
}
