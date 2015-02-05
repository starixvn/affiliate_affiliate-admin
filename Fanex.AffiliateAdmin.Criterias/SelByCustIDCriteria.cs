namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Select By Customer Id Criteria.
    /// </summary>
    public class SelByCustIDCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_SelByCustID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}