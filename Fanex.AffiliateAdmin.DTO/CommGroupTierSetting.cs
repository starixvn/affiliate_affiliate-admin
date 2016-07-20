namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Commission Group Detail Class.
    /// </summary>
    public class CommGroupTierSetting
    {
        public int GroupId { get; set; }

        public byte ProductID { get; set; }

        public string ProductName { get; set; }

        public byte TierID { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }

        public int MinQM { get; set; }

        public int MaxQM { get; set; }

        public decimal Commission { get; set; }
    }
}