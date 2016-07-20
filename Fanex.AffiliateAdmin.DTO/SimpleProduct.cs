namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Product class.
    /// </summary>
    public class SimpleProduct
    {
        public byte ProductId { get; set; }

        public string ProductName { get; set; }

        public string OriginalProductName { get; set; }

        public int TotalQualifiedMembers { get; set; }
    }
}