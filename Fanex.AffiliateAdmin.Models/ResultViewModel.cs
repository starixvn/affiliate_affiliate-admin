using Fanex.AffiliateAdmin.Core.Constants;

namespace Fanex.AffiliateAdmin.Models
{
    /// <summary>
    /// Result View Model.
    /// </summary>
    public class ResultViewModel
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSuccess => ErrorCode == ErrorCodes.Success;
    }
}