namespace Fanex.AffiliateAdmin.Criterias.Configuration.AffType
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Detail criteria class.
    /// </summary>
    public class AffTypeDetailCriteria : CriteriaBase
    {
        public int ID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffTypeSelByID";
        }

        public override bool IsValid()
        {
            return ID > 0;
        }
    }
}