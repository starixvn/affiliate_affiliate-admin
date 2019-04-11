namespace Fanex.AffiliateAdmin.DataAccess
{
    using Criterias;

    /// <summary>
    /// Interface AffiliateDataAccess.
    /// </summary>
    public interface IBetListDataAccess
    {
        decimal GetExchangeRate(GetExchangeRateCriteria criteria);
    }
}