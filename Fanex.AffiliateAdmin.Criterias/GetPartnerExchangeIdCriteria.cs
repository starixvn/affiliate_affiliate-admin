namespace Fanex.AffiliateAdmin.Criterias
{
    using Data.Repository;

    public class GetPartnerExchangeIdCriteria : CriteriaBase
    {
        public GetPartnerExchangeIdCriteria(string currencies, byte currencyType = 1)
        {
            Currencies = currencies;
            CurrencyType = currencyType;
        }

        public string Currencies { get; set; }

        public byte CurrencyType { get; set; }

        public override string GetSettingKey() => "Aff_Get_PartnerExchangeID";

        public override bool IsValid() => true;
    }
}
