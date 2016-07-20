namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MediaType
    {
        public short TypeId { get; set; }

        public string Value { get; set; }

        public short? ParentId { get; set; }

        public int HasSubCategory { get; set; }
    }
}