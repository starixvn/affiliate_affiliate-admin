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
    using System.Diagnostics.CodeAnalysis;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Criterias.Account;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// Interface Account service.
    /// </summary>
    public interface IAccountService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        ResultViewModel LoginInternal(LoginInternalCriteria criteria, out Account account);

        /// <summary>
        /// Get statistics for Affiliate.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return object.</returns>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        ResultViewModel Login(LoginCriteria criteria, out Account account, string logInToken);

        /// <summary>
        /// Get by username.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <returns>Account value.</returns>
        Account GetByUsername(string userName);

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

        int CheckIn(CheckInCriteria criteria);

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

        Account GetAccountInfo(SelByCustIDCriteria criteria);

        /// <summary>
        /// Update account.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void AccountUpdate(AccountUpdateCriteria criteria);

        /// <summary>
        /// Insert new account.
        /// </summary>
        /// <param name="criteria">Account criteria.</param>
        void AccountInsert(AccountInsCriteria criteria);

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