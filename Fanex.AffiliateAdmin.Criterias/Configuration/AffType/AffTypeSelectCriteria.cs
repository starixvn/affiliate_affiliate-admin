namespace Fanex.AffiliateAdmin.Criterias.Configuration.AffType
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Select criteria class.
    /// </summary>
    public class AffTypeSelectCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffTypeSelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}