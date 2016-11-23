namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Script.Serialization;
    using Common;
    using Common.Helper;
    using Core.Enumerations;
    using Core.Services;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Criterias.Account;
    using Fanex.AffiliateAdmin.DataAccess;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Newtonsoft.Json;

    /// <summary>
    /// Account Service.
    /// </summary>
    public class AccountService : QueryService, IAccountService
    {
        private const int WrongUserPasswordErrorCode = 101;

        private readonly IAccountDataAccess _accountDataAccess;

        private readonly ISystemSettingService _systemSettingService;

        public AccountService()
            : this(null, null)
        {
        }

        private AccountService(IAccountDataAccess accountDataAccess, ISystemSettingService systemSettingService)
        {
            _accountDataAccess = accountDataAccess ?? new AccountDataAccess();

            _systemSettingService = systemSettingService ?? new SystemSettingService();
        }

        public ResultViewModel LoginInternal(LoginInternalCriteria criteria, out Account account)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return _accountDataAccess.LoginInternal(criteria, out account);
        }

        public ResultViewModel Login(LoginCriteria criteria, out Account account, string logInToken)
        {
            ResultViewModel result = null;
            account = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            var user = GetByUsername(criteria.Username);
            if (user != null)
            {
                var hashPassword = Cryptography.CreateHash(user.Password + logInToken, user.UserSalt);

                if (criteria.UserPwd.Equals(hashPassword, StringComparison.OrdinalIgnoreCase))
                {
                    result = _accountDataAccess.Login(criteria, out account);
                }
            }

            return result ?? new ResultViewModel()
            {
                ErrorCode = WrongUserPasswordErrorCode
            };
        }

        /// <summary>
        /// Get by username.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <returns>User value.</returns>
        public Account GetByUsername(string userName)
        {
            var param = new
            {
                Username = userName,
            };

            using (var objectDb = new ObjectDb("AffAdmin_UserInfoSelByUserName"))
            {
                var user = objectDb.Query<User>(param).FirstOrDefault();

                Account account = null;

                if (user != null)
                {
                    account = new Account
                    {
                        Username = user.Username,
                        Password = user.Password,
                        UserSalt = user.UserSalt
                    };
                }

                return account;
            }
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

        public int CheckIn(CheckInCriteria criteria)
        {
            return _accountDataAccess.CheckIn(criteria);
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
        public void AccountUpdate(AccountUpdateCriteria criteria)
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

        public ICurrentSession CreateSession(Account user, int licenseeId, AppMode appMode = AppMode.Self)
        {
            var policies = GetUserPolicyList(new UserPolicyCriteria
            {
                CustID = user.CustID,
                LicenseeId = licenseeId
            });
            var serializer = new JavaScriptSerializer();
            var policyString = serializer.Serialize(policies);
            Logging.LogService.Info(policyString);
            var currencyList = _systemSettingService.GetAffSystemSupportedCurrencyList(new SystemSupportedCurrencyCriteria
            {
                UserId = user.CustID,
                LicenseeId = licenseeId
            }).ToList();

            var menuList = GetMenuList();
            return new CurrentSession(user, policies, currencyList, menuList);
        }

        private IEnumerable<FunctionPolicy> GetUserPolicyList(UserPolicyCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<FunctionPolicy>(criteria);
        }

        private IEnumerable<FunctionMenu> GetMenuList()
        {
            const string FunctionFilePath = "~/App_Data/SystemFunction.json";

            var fileContent = File.ReadAllText(HostingEnvironment.MapPath(FunctionFilePath));
            if (!string.IsNullOrWhiteSpace(fileContent))
            {
                return JsonConvert.DeserializeObject<IList<FunctionMenu>>(fileContent);
            }

            return new List<FunctionMenu>();
        }
    }
}