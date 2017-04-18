namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Common;
    using Common.Helper;
    using Core.Enumerations;
    using Core.Services;
    using Core.Services.Interface;
    using Core.Session;
    using Criterias;
    using DTO;
    using DTO.OneAuth;
    using Fanex.AffiliateAdmin.Criterias.Account;
    using Fanex.AffiliateAdmin.DataAccess;
    using Fanex.AffiliateAdmin.DTO.Common;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Newtonsoft.Json;

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

        public Account GetByUsername(string userName, int licenseeId = 0, bool isInternal = false)
        {
            var param = new
            {
                Username = userName,
                LicenseeId = licenseeId,
                isInternal = isInternal
            };

            using (var objectDb = new ObjectDb("AffAdmin_UserInfoSelByUserName"))
            {
                return objectDb.QueryEntity<Account>(param);
            }
        }

        public void UserLogOut(LogOutCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            _accountDataAccess.UserLogOut(criteria);
        }

        public int CheckIn(CheckInCriteria criteria)
        {
            return _accountDataAccess.CheckIn(criteria);
        }

        public bool ReCheckIn(ReCheckInCriteria criteria)
        {
            return _accountDataAccess.ReCheckIn(criteria);
        }

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

        public Account GetAccountInfo(SelByCustIDCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var data = GetData<Account>(criteria);

            return data.FirstOrDefault();
        }

        public void AccountUpdate(AccountUpdateCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AccountInsert(AccountInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AccountUpdatePassword(ChangePasswordCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AccountUpdatePolicy(ICriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AccountUpdatePassword(AccountPasswordCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public ICurrentSession CreateOneAuthSession(OneAuthToken token, Account user, int licenseeId)
        {
            IEnumerable<FunctionPolicy> policies;
            IEnumerable<Currency> currencies;
            var allCurrencies = _systemSettingService.GetLicenseeSupportedCurrencyList(new LicenseeSupportedCurrencyCriteria
            {
                LicenseeId = licenseeId
            });

            if (token.IsAdminstrator)
            {
                currencies = allCurrencies;
                policies = GetSystemFunctions();
            }
            else
            {
                currencies = allCurrencies.Where(x => token.Currency.Any(y => y.Currency.Equals(x.CurrencyCode, StringComparison.OrdinalIgnoreCase))).ToList();
                policies = token.Permissions.Select(x => new FunctionPolicy
                {
                    FuncName = x.FunctionKey,
                    Actual_Policy = FunctionPolicy.HostPolicy,
                    Policy = FunctionPolicy.HostPolicy,
                    GPolicy = FunctionPolicy.HostPolicy
                });
            }

            var session = new CurrentSession(user, policies, currencies)
            {
                ApplicationMode = AppMode.Parasitic
            };

            return session;
        }

        public ICurrentSession CreateInternalSession(Account user, int licenseeId)
        {
            var policies = GetUserPolicyList(new UserPolicyCriteria
            {
                CustID = user.CustID,
                LicenseeId = licenseeId
            });
            var currencies = _systemSettingService.GetAffSystemSupportedCurrencyList(new SystemSupportedCurrencyCriteria
            {
                UserId = user.CustID,
                LicenseeId = licenseeId
            }).ToList();

            var session = new CurrentSession(user, policies, currencies)
            {
                ApplicationMode = AppMode.Self
            };

            return session;
        }

        public ResultInfo SyncUserInfo(string licenseeName, string userName)
        {
            const int AdminGroup = 0;
            const string UspName = @"Aff_SyncInternalUser";

            const string DefaultPass = "J0NilQZC1ywhNqNu8XMBqIvesaErM+DGia1za8P+tH8=";
            Logging.Logger.Log.Info($"Synced user from LAP system - userName={userName}");
            using (var db = new ObjectDb(UspName))
            {
                db.AddInParameter("LicenseeName", DbType.String, licenseeName);
                db.AddInParameter("Username", DbType.String, userName);
                db.AddInParameter("EncryptedPassword", DbType.String, DefaultPass);
                db.AddInParameter("AffGroup", DbType.Int32, AdminGroup);
                db.AddOutParameter("ErrCode", DbType.Int32, 4);
                db.AddOutParameter("ErrMsg", DbType.String, 4000);

                db.ExecuteNonQuery();

                var errorCode = db.GetParameterValue("ErrCode");
                var errorMessage = db.GetParameterValue("ErrMsg");

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = Converter.ToString2(errorMessage)
                };
            }
        }

        private IEnumerable<FunctionPolicy> GetUserPolicyList(UserPolicyCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<FunctionPolicy>(criteria);
        }

        private IEnumerable<FunctionPolicy> GetSystemFunctions()
        {
            const string FunctionFilePath = "~/App_Data/SystemFunction.json";

            var fileContent = File.ReadAllText(HostingEnvironment.MapPath(FunctionFilePath));
            if (!string.IsNullOrWhiteSpace(fileContent))
            {
                var policyList = JsonConvert.DeserializeObject<IList<FunctionPolicy>>(fileContent);
                foreach (var item in policyList)
                {
                    item.Actual_Policy = FunctionPolicy.HostPolicy;
                    item.Policy = FunctionPolicy.HostPolicy;
                    item.GPolicy = FunctionPolicy.HostPolicy;
                }

                return policyList;
            }

            return new List<FunctionPolicy>();
        }
    }
}