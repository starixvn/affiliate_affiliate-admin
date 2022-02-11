namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Simple Criteria Base.
    /// </summary>
    public class SimpleCriteriaBase : CriteriaBase
    {
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}