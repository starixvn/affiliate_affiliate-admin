namespace Fanex.AffiliateAdmin.DataAccess
{
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Models;

    /// <summary>
    /// Interface Account.
    /// </summary>
    public interface IAuthDataAccess
    {
        ResultViewModel Login(LoginCriteria criteria, out Account account);

        /// <summary>
        /// User Logout.
        /// </summary>
        /// <param name="criteria">Logout criteria.</param>
        void UserLogOut(LogOutCriteria criteria);

        /// <summary>
        /// Check in after login to get On User Id.
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <returns>The On user id.</returns>
        int CheckIn(CheckInCriteria criteria);

        /// <summary>
        /// Auto kick out previous login user.
        /// </summary>
        /// <param name="criteria">Re-CheckIn Criteria.</param>
        /// <returns>Return boolean.</returns>
        bool ReCheckIn(ReCheckInCriteria criteria);
    }
}