namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class PaymentLogCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string LogContent { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_WebAPI_WriteLog";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}