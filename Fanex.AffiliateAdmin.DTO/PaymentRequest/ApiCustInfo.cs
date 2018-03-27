namespace Fanex.AffiliateAdmin.DTO.PaymentRequest
{
    public class ApiCustInfo
    {
        public string IbcUserName { get; set; } = string.Empty;

        public string PlayerCurrency { get; set; } = string.Empty;

        public string ErrorMessage { get; set; }
    }
}
