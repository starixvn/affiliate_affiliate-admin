namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Control Select By Id Criteria.
    /// </summary>
    public class DomainControlSelByIdCriteria : CriteriaBase
    {
        public int ID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainControlSelById";
        }

        public override bool IsValid()
        {
            return ID > 0;
        }
    }
}