namespace Fanex.AffiliateAdmin.Criterias
{
    using Data.Repository;

    public class WriteLogBaseCriteria : CriteriaBase
    {
        public int Creator { get; set; }

        public int AffId { get; set; }

        public string SpName { get; set; }

        public string Details { get; set; }

        public string Ip { get; set; }

        public override string GetSettingKey() => string.Empty;

        public override bool IsValid() => true;
    }
}
