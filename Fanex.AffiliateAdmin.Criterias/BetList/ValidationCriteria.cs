namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class ValidationCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public string MemberName { get; set; }

        public bool BelongLevel1 { get; set; }

        public bool BelongLevel2 { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_BetList_CheckBelonging";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}