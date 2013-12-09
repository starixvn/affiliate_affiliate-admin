// --------------------------------------------------------------------------
// <copyright file="IAccountDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.30">Created</change>
//   <change who="Kaden.Trinh" date="2013.06.10">Implement funcion get statistics overview account</change>
//   <change who="Kaden.Trinh" date="2013.06.11">Implement funcion get Current Commission Setting</change>
// </history>
namespace Fanex.AffiliateAdmin.DataAccess.Interface
{
    using Criterias;
    using DTO;

    /// <summary>
    /// Interface Account.
    /// </summary>
    public interface IAccountDataAccess
    {
        /// <summary>
        /// Get statistics for Affiliate.
        /// </summary>
        /// <param name="criteria">Login Criteria.</param>
        /// <returns>Return Account.</returns>
        Account Login(LoginCriteria criteria);

        /// <summary>
        /// User Logout.
        /// </summary>
        /// <param name="criteria">Logout criteria.</param>
        void UserLogOut(LogOutCriteria criteria);

        /// <summary>
        /// Auto kick out previous login user.
        /// </summary>
        /// <param name="criteria">Re-CheckIn Criteria.</param>
        /// <returns>Return boolean.</returns>
        bool ReCheckIn(ReCheckInCriteria criteria);
    }
}