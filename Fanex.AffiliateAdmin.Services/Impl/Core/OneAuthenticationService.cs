namespace Fanex.AffiliateAdmin.Services.Impl.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Security;
    using AffiliateAdmin.Core;
    using AffiliateAdmin.Core.Configurations;
    using AffiliateAdmin.Core.Enumerations;
    using AffiliateAdmin.Core.Session;
    using DTO;
    using DTO.Common;
    using Fanex.AffiliateAdmin.Core.Constants;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Caching;
    using Interfaces;

    public class OneAuthenticationService : IOneAuthenticationService
    {
        private static readonly IDictionary<string, string> MapLanguages = new Dictionary<string, string>
        {
            { "en-US", "en" },
            { "zh-TW", "ch" },
            { "zh-CN", "cs" },
            { "zh-Hans", "zhcn" },
            { "ja-JP", "jp" },
            { "th-TH", "th" },
            { "ko-KR", "ko" },
            { "vi-VN", "vn" }
        };

        private readonly IAccountService _accountService;

        public OneAuthenticationService() : this(null)
        {
        }

        public OneAuthenticationService(IAccountService accountService)
        {
            _accountService = accountService ?? new AccountService();
        }

        protected internal virtual string GetStringFromFile(string path)
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(path)))
            {
                return reader.ReadToEnd();
            }
        }

        public ResultInfo SignIn(HttpContextBase currentContext, OneAuthenticationToken tokenInfo)
        {
            if (!tokenInfo.SiteName.Equals(AppSettings.SystemName, StringComparison.OrdinalIgnoreCase))
            {
                return new ResultInfo
                {
                    Code = int.Parse(OneAuthorizationErrorCode.LicenseeIsNotExisted),
                    Message = Lang.LicenseeIsNotExisted
                };
            }

            var language = MapLanguages.Where(i => i.Value.Equals(tokenInfo.Language))
                       .Select(i => i.Key).DefaultIfEmpty<string>("en-Us").FirstOrDefault();
            var currentSession = SessionManager.CurrentSession;
            if (IsAuthenticated(currentContext, currentSession))
            {
                var isSameSession = VerifySessionWithToken(currentSession, tokenInfo);
                if (isSameSession)
                {
                    if (!language.Equals(AppSession.Language))
                    {
                        AppSession.Language = language;
                    }

                    return new ResultInfo
                    {
                        ExtendedData = tokenInfo.ActionLink
                    };
                }
            }

            var userCheckingResult = CheckAndSyncUserInfo(AppSettings.LicenseeID, tokenInfo.SiteName, tokenInfo.UserName);

            if (!userCheckingResult.IsSuccessful)
            {
                return userCheckingResult;
            }

            tokenInfo.UserSEQ = int.Parse(userCheckingResult.ExtendedData);
            return SetAuthorize(tokenInfo);
        }

        private ResultInfo CheckAndSyncUserInfo(int licenseeId, string siteName, string userName)
        {
            var temp = _accountService.GetByUsername(userName, licenseeId, true);
            if (temp != null)
            {
                return new ResultInfo
                {
                    ExtendedData = temp.CustID.ToString()
                };
            }

            var result = _accountService.SyncUserInfo(siteName, userName);
            return result.IsSuccessful ? CheckAndSyncUserInfo(licenseeId, siteName, userName) : new ResultInfo
            {
                Code = int.Parse(OneAuthorizationErrorCode.NotFoundUser),
                Message = Lang.NotFoundUser
            };
        }

        public OneAuthenticationToken GetTokenInfo(string token)
        {
            var oneBookService = new OneBookSecurityWrapper();
            return oneBookService.Authenticate(token);
        }

        protected internal virtual ResultInfo SetAuthorize(OneAuthenticationToken tokenInfo)
        {
            CreateSession(tokenInfo);
            var authName = $"{tokenInfo.UserName}_{tokenInfo.VendorID}_{tokenInfo.SiteName}";
            FormsAuthentication.SetAuthCookie(authName, false);

            return new ResultInfo
            {
                ExtendedData = tokenInfo.ActionLink
            };
        }

        [CLSCompliant(false)]
        protected internal virtual ICache GetMemCacheService() => CacheService.CreateInstance(CacheType.MemoryCache);

        private static bool VerifySessionWithToken(ICurrentSession currentSession, OneAuthenticationToken tokenInfo)
        {
            return currentSession.Username == tokenInfo.UserName
                && currentSession.VendorID == tokenInfo.VendorID && currentSession.VendorIdentityID == tokenInfo.UserID;
        }

        private static bool IsAuthenticated(HttpContextBase currentContext, ICurrentSession currentSession)
        {
            var identity = currentContext.User.Identity;
            var identityValidation = identity != null && identity.IsAuthenticated && !string.IsNullOrEmpty(identity.Name);

            var sessionValidation = currentSession != null && currentSession.CustID > 0 &&
                !string.IsNullOrEmpty(currentSession.VendorID) && !string.IsNullOrEmpty(currentSession.VendorIdentityID)
                && !currentSession.IsIT && currentSession.ApplicationMode == AppMode.Parasitic;

            return identityValidation && sessionValidation;
        }

        private void CreateSession(OneAuthenticationToken tokenInfo)
        {
            var user = new Account
            {
                CustID = tokenInfo.UserSEQ,
                IsIT = false,
                Username = tokenInfo.UserName
            };
            var session = _accountService.CreateSession(user, AppSettings.LicenseeID, AppMode.Parasitic);
            session.VendorID = tokenInfo.VendorID;
            session.VendorIdentityID = tokenInfo.UserID;
            SessionManager.CurrentSession = session;
        }
    }
}