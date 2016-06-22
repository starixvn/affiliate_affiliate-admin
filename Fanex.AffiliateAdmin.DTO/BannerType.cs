namespace Fanex.AffiliateAdmin.DTO
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Banner Type class.
    /// </summary>
    public class BannerType
    {
        public short TypeID { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Allow Type Property")]
        public string Type { get; set; }

        public string Value { get; set; }

        public byte? DisplayOrder { get; set; }

        public short? ParentID { get; set; }
    }
}