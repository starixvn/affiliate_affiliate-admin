namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;

    /// <summary>
    /// Interface Affiliate service.
    /// </summary>
    public interface IBetListService
    {
        decimal GetExchangeRate(GetExchangeRateCriteria criteria);

        void ValidatePermission(ValidationCriteria criteria);
    }
}