namespace Fanex.AffiliateAdmin.Models
{
    /// <summary>
    /// Result View Model.
    /// </summary>
    public class ResultViewModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}