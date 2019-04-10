namespace Fanex.AffiliateAdmin.Services
{
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using Fanex.AffiliateAdmin.DataAccess;

    /// <summary>
    /// Affiliate Service.
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IBetListDataAccess _betlistDataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        public BetListService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        /// <param name="betlistDataAccess">Account data access.</param>
        private BetListService(IBetListDataAccess betlistDataAccess)
        {
            _betlistDataAccess = betlistDataAccess ?? new BetListDataAccess();
        }

        public decimal GetExchangeRate(GetExchangeRateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return _betlistDataAccess.GetExchangeRate(criteria);
        }

        public void ValidatePermission(ValidationCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}