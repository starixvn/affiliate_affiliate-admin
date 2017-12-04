namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class PaymentCheckInProgressCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PaymentRequest_InProgress";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}