namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerListCriteria : CriteriaBase
    {
        public short? Category { get; set; }

        public short? SubCategory { get; set; }

        public string Language { get; set; }

        public short? Currency { get; set; }

        [SpParam(SqlParamName = "Type")]
        public short? AffType { get; set; }

        public short? ImageType { get; set; }

        public short? Size { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerSel";
        }

        public override bool IsValid()
        {
            if (Category == null && SubCategory != null)
            {
                return false;
            }

            return true;
        }
    }
}