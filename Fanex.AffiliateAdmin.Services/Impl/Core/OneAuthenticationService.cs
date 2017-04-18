namespace Fanex.AffiliateAdmin.Services.Impl.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Security;
    using AffiliateAdmin.Core.Configurations;
    using AffiliateAdmin.Core.Enumerations;
    using AffiliateAdmin.Core.Session;
    using DTO;
    using DTO.Common;
    using DTO.OneAuth;
    using Fanex.AffiliateAdmin.Core.Constants;
    using Fanex.AffiliateAdmin.Resources;
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

        private readonly IAuthService _authService;

        private readonly IOneBookSecurityWrapper _oneBookSecurityWrapper;

        public OneAuthenticationService() : this(null, null)
        {
        }

        public OneAuthenticationService(IAuthService authService, IOneBookSecurityWrapper oneBookSecurityWrapper)
        {
            _authService = authService ?? new AuthService();
            _oneBookSecurityWrapper = oneBookSecurityWrapper ?? new OneBookSecurityWrapper();
        }

        protected internal virtual string GetStringFromFile(string path)
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(path)))
            {
                return reader.ReadToEnd();
            }
        }

        public ResultInfo SignIn(HttpContextBase currentContext, OneAuthToken tokenInfo)
        {
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

            var userCheckingResult = CheckAndSyncUserInfo(AppSettings.LicenseeID, tokenInfo.SiteName, tokenInfo.UserID);

            if (!userCheckingResult.IsSuccessful)
            {
                return userCheckingResult;
            }

            tokenInfo.UserSEQ = int.Parse(userCheckingResult.ExtendedData);
            return SetAuthorize(tokenInfo);
        }

        public OneAuthToken GetTokenInfo(string token)
        {
            return _oneBookSecurityWrapper.Authenticate(token);
        }

        protected internal virtual ResultInfo SetAuthorize(OneAuthToken tokenInfo)
        {
            CreateSession(tokenInfo);
            var authName = $"{tokenInfo.UserID}_{tokenInfo.VendorID}_{tokenInfo.SiteName}";
            FormsAuthentication.SetAuthCookie(authName, false);

            return new ResultInfo
            {
                ExtendedData = tokenInfo.ActionLink
            };
        }

        private static bool VerifySessionWithToken(ICurrentSession currentSession, OneAuthToken tokenInfo)
        {
            return currentSession.Username == tokenInfo.UserID
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

        private void CreateSession(OneAuthToken tokenInfo)
        {
            var user = new Account
            {
                CustID = tokenInfo.UserSEQ,
                IsIT = false,
                Username = tokenInfo.UserID
            };
            var session = _authService.CreateOneAuthSession(tokenInfo, user, AppSettings.LicenseeID);
            session.VendorID = tokenInfo.VendorID;
            session.VendorIdentityID = tokenInfo.UserID;
            SessionManager.CurrentSession = session;
        }

        private ResultInfo CheckAndSyncUserInfo(int licenseeId, string siteName, string userName)
        {
            var result = GetUser(licenseeId, userName);
            if (result.IsSuccessful)
            {
                return result;
            }

            result = _authService.SyncUserInfo(siteName, userName);

            return result.IsSuccessful ? GetUser(licenseeId, userName) : new ResultInfo
            {
                Code = OneAuthorizationErrorCode.NotFoundUser,
                Message = Lang.NotFoundUser
            };
        }

        private ResultInfo GetUser(int licenseeId, string userName)
        {
            var user = _authService.GetByUsername(userName, licenseeId, true);
            if (user != null)
            {
                return new ResultInfo
                {
                    ExtendedData = user.CustID.ToString()
                };
            }

            return new ResultInfo
            {
                Code = OneAuthorizationErrorCode.NotFoundUser,
                Message = Lang.NotFoundUser
            };
        }
    }
}