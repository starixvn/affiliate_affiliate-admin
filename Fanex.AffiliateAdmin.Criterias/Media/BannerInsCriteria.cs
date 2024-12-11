namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerInsCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public HttpPostedFileBase File { get; set; }

        public int BannerId { get; set; }

        public string Content { get; set; }

        public short Category { get; set; }

        [SpParam(SqlParamName = "BannerType")]
        public short AffType { get; set; }

        public string Language { get; set; }

        public short Currency { get; set; }

        public short ImageType { get; set; }

        [SpParam(Ignored = true)]
        public string FileName { get; set; }

        public string Size { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}