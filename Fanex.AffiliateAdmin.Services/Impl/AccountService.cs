// --------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using Common;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.DataAccess;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.Repository;

    /// <summary>
    /// Account Service.
    /// </summary>
    public class AccountService : QueryService, IAccountService
    {
        #region Fields

        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IAccountDataAccess _accountDataAccess;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService" /> class.
        /// </summary>
        public AccountService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService" /> class.
        /// </summary>
        /// <param name="accountDataAccess">Account data access.</param>
        private AccountService(IAccountDataAccess accountDataAccess)
        {
            _accountDataAccess = accountDataAccess ?? new AccountDataAccess();
        }

        #endregion Constructor

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="criteria">Login criteria.</param>
        /// <returns>Account info.</returns>
        public Account Login(LoginCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);

            return _accountDataAccess.Login(criteria);
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        public void UserLogOut(LogOutCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            _accountDataAccess.UserLogOut(criteria);
        }

        /// <summary>
        /// Get role access of user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List function policy.</returns>
        public IEnumerable<FunctionPolicy> UserPolicySel(UserPolicyCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<FunctionPolicy>(criteria);
        }

        /// <summary>
        /// ReCheckIn: kick out the previous login user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return Is Kicked out.</returns>
        public bool ReCheckIn(ReCheckInCriteria criteria)
        {
            return _accountDataAccess.ReCheckIn(criteria);
        }

        /// <summary>
        /// Get List Account Management.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List Account.</returns>
        public IEnumerable<Account> GetListAccountManagement(SelByGroupIDCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = GetData<Account>(criteria);

            foreach (var account in result)
            {
                var translated = Lang.ResourceManager.GetString(account.GroupName.Replace(" ", string.Empty));
                account.GroupName = string.IsNullOrEmpty(translated) ? account.GroupName : translated;
            }

            return result;
        }

        /// <summary>
        /// Get account info.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Account info.</returns>
        public Account GetAccountInfo(SelByCustIDCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var data = GetData<Account>(criteria);

            return data.FirstOrDefault();
        }

        /// <summary>
        /// Update account.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        public void AccountUpdate(AccountUpdCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Insert new account.
        /// </summary>
        /// <param name="criteria">Account criteria.</param>
        public void AccountInsert(AccountInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update status.
        /// </summary>
        /// <param name="criteria">Account criteria.</param>
        public void AccountUpdateStatus(AccountUpdClosedCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update Account Password.
        /// </summary>
        /// <param name="criteria">Change password criteria.</param>
        public void AccountUpdatePassword(ChangePasswordCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update Account Policy.
        /// </summary>
        /// <param name="criteria">Policy update criteria.</param>
        public void AccountUpdatePolicy(ICriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Admin update him/herself password.
        /// </summary>
        /// <param name="criteria">Account Password Criteria.</param>
        public void AccountUpdatePassword(AccountPasswordCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}