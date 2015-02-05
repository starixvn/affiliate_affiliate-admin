// --------------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.30">Created</change>
//   <change who="Kaden.Trinh" date="2013.06.11">Implement funcion get Current Commission Setting</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Fanex.Data.Repository;

    /// <summary>
    /// Interface Account service.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Get statistics for Affiliate.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return object.</returns>
        Account Login(LoginCriteria criteria);

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void UserLogOut(LogOutCriteria criteria);

        /// <summary>
        /// Get role access of user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List function policy.</returns>
        IEnumerable<FunctionPolicy> UserPolicySel(UserPolicyCriteria criteria);

        /// <summary>
        /// Auto kicked out previous user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return boolean Is kicked out.</returns>
        bool ReCheckIn(ReCheckInCriteria criteria);

        /// <summary>
        /// Get List Account Management.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List Account.</returns>
        IEnumerable<Account> GetListAccountManagement(SelByGroupIDCriteria criteria);

        /// <summary>
        /// Get account info.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Account information.</returns>
        Account GetAccountInfo(SelByCustIDCriteria criteria);

        /// <summary>
        /// Update account.
        /// </summary>
        /// <param name="criteria">Account criteria.</param>
        void AccountUpdate(AccountUpdCriteria criteria);

        /// <summary>
        /// Insert new account.
        /// </summary>
        /// <param name="criteria">Account criteria.</param>
        void AccountInsert(AccountInsCriteria criteria);

        /// <summary>
        /// Update account status.
        /// </summary>
        /// <param name="criteria">Account criteria.</param>
        void AccountUpdateStatus(AccountUpdClosedCriteria criteria);

        /// <summary>
        /// Update account password.
        /// </summary>
        /// <param name="criteria">Change password criteria.</param>
        void AccountUpdatePassword(ChangePasswordCriteria criteria);

        /// <summary>
        /// Update account policy.
        /// </summary>
        /// <param name="criteria">Policy update criteria.</param>
        void AccountUpdatePolicy(ICriteria criteria);

        /// <summary>
        /// Admin update him/herself password.
        /// </summary>
        /// <param name="criteria">Account Password Criteria.</param>
        void AccountUpdatePassword(AccountPasswordCriteria criteria);
    }
}