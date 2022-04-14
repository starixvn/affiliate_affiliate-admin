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
    using Data;
    using DataAccess;
    using DTO;
    using DTO.Common;
    using DTO.OneAuth;
    using Fanex.Data.SqlServer;
    using Models;
    using Newtonsoft.Json;

    public class AuthService : QueryService, IAuthService
    {
        private const int WrongUserPasswordErrorCode = 101;

        private readonly IAuthDataAccess _authDataAccess;

        private readonly ISystemSettingService _systemSettingService;

        public AuthService()
            : this(null, null)
        {
        }

        private AuthService(IAuthDataAccess accountDataAccess, ISystemSettingService systemSettingService)
        {
            _authDataAccess = accountDataAccess ?? new AuthDataAccess();

            _systemSettingService = systemSettingService ?? new SystemSettingService();
        }

        public ResultViewModel Login(LoginCriteria criteria, out Account account, string logInToken)
        {
            ResultViewModel result = null;
            account = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            var user = GetByUsername(criteria.Username);
            if (user != null)
            {
                var hashPassword = Common.Cryptography.CreateHash(user.Password + logInToken, user.UserSalt);

                if (criteria.UserPwd.Equals(hashPassword, StringComparison.OrdinalIgnoreCase))
                {
                    result = _authDataAccess.Login(criteria, out account);
                }
            }

            return result ?? new ResultViewModel()
            {
                ErrorCode = WrongUserPasswordErrorCode
            };
        }

        public void LogOut(LogOutCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            _authDataAccess.UserLogOut(criteria);
        }

        public int CheckIn(CheckInCriteria criteria)
        {
            return _authDataAccess.CheckIn(criteria);
        }

        public bool ReCheckIn(ReCheckInCriteria criteria)
        {
            return _authDataAccess.ReCheckIn(criteria);
        }

        public Account GetByUsername(string userName, int licenseeId = 0, bool isInternal = false)
        {
            var param = new
            {
                Username = userName,
                LicenseeId = licenseeId,
                isInternal
            };

            using (var objectDb = new SqlObjectDb("AffAdmin_UserInfoSelByUserName"))
            {
                return objectDb.QueryEntity<Account>(param);
            }
        }

        public ICurrentSession CreateOneAuthSession(OneAuthToken token, Account user)
        {
            var currencyList = GetCurrencyList(token);
            var policyList = GetPolicyList(token);
            return new CurrentSession(user, policyList, currencyList)
            {
                ApplicationMode = AppMode.Parasitic
            };
        }

        public void UpdateOneAuthSession(ICurrentSession session, OneAuthToken token)
        {
            var currencyList = GetCurrencyList(token);
            var policyList = GetPolicyList(token);
            session.SetPermissions(currencyList, policyList);
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

        private IEnumerable<Currency> GetCurrencyList(OneAuthToken token)
        {
            var allCurrencies = _systemSettingService.GetLicenseeSupportedCurrencyList(new LicenseeSupportedCurrencyCriteria());

            return  allCurrencies.Where(x => token.Currency.Any(y => y.CurrencyId == x.OriginalCurrencyID)).ToList();
        }

        private IEnumerable<FunctionPolicy> GetPolicyList(OneAuthToken token)
        {
            return token.IsAdminstrator ? GetSystemFunctions() : token.Permissions.Select(x => new FunctionPolicy
            {
                FuncName = x.FunctionKey,
                Actual_Policy = FunctionPolicy.HostPolicy,
                Policy = FunctionPolicy.HostPolicy,
                GPolicy = FunctionPolicy.HostPolicy
            }).ToList();
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