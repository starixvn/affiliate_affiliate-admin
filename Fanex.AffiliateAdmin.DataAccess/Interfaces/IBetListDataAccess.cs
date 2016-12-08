namespace Fanex.AffiliateAdmin.DataAccess
{
    using Criterias;
    using DTO;
    using Models;

    /// <summary>
    /// Interface AffiliateDataAccess.
    /// </summary>
    public interface IBetListDataAccess
    {
        decimal GetExchangeRate(GetExchangeRateCriteria criteria);

        CustomerInfoBetList GetPlayerIbcInfo(string playerUsername);
    }
}