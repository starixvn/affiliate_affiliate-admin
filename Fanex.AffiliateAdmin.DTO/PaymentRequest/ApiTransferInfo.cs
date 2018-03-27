namespace Fanex.AffiliateAdmin.DTO.PaymentRequest
{
    public class ApiTransferInfo
    {
        public long IbcRefNo { get; set; }

        public long IbcTransId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
