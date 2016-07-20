namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DummyAccIdCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_DummyAccID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}