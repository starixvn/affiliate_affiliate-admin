namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public abstract class BaseCriteria : CriteriaBase
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int NRows { get; set; }
    }
}
