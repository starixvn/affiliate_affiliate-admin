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
    using AffiliateAdmin.Core.Constants;
    using AffiliateAdmin.Core.Enumerations;
    using AffiliateAdmin.Core.Session;
    using DTO;
    using DTO.Common;
    using Fanex.Caching;
    using Interface;
    using Newtonsoft.Json;
    using Resources;

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

        public ResultInfo SignIn(HttpContextBase currentContext, int licenseeId, OneAuthenticationToken tokenInfo)
        {
            var language = MapLanguages.Where(i => i.Value.Equals(tokenInfo.Language))
                       .Select(i => i.Key).DefaultIfEmpty<string>("en-Us").FirstOrDefault();
            var currentSession = SessionManager.CurrentSession;
            if (IsAuthenticated(currentContext, currentSession))
            {
                var isSameSession = VerifySessionWithToken(currentSession, tokenInfo, licenseeId);
                if (isSameSession)
                {
                    if (!language.Equals(AppSession.Language))
                    {
                        AppSession.Language = language;
                        var langManager = new LanguageManager();
                        langManager.SetCulture(language);
                    }

                    return new ResultInfo
                    {
                        ExtendedData = tokenInfo.ActionLink
                    };
                }
            }

            return SetAuthorize(licenseeId, tokenInfo);
        }

        public LicenseeSiteName SingleLicenseeSiteName(string siteName)
        {
            const string SiteNameFilePath = "~/App_Data/LicenseeSiteName.json";
            var defaultValue = new LicenseeSiteName();
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                var items = GetLiceseeSiteNameList(SiteNameFilePath);

                if (items != null)
                {
                    var item = items.FirstOrDefault(i => i.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase));
                    return item != null ? item : defaultValue;
                }
            }

            return defaultValue;
        }

        public OneAuthenticationToken GetTokenInfo(string token)
        {
            var oneBookService = new OneBookSecurityWrapper();
            return oneBookService.Authenticate(token);
        }

        protected internal virtual ResultInfo SetAuthorize(int licenseeId, OneAuthenticationToken tokenInfo)
        {
            if (AppSettings.LicenseeID != licenseeId)
            {
                return new ResultInfo
                {
                    Code = int.Parse(OneAuthorizationErrorCode.LicenseeIsNotExisted),
                    Message = Lang.LicenseeIsNotExisted
                };
            }

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

        private static bool VerifySessionWithToken(ICurrentSession currentSession, OneAuthenticationToken tokenInfo, int licenseeId)
        {
            return currentSession.CustID == tokenInfo.UserSEQ && currentSession.Username == tokenInfo.UserName
                && currentSession.VendorID == tokenInfo.VendorID && currentSession.VendorIdentityID == tokenInfo.UserID
                && AppSettings.LicenseeID == licenseeId;
        }

        private static bool IsAuthenticated(HttpContextBase currentContext, ICurrentSession currentSession)
        {
            var identity = currentContext.User.Identity;
            var identityValidation = identity != null && identity.IsAuthenticated && !string.IsNullOrEmpty(identity.Name);

            var sessionValidation = currentSession != null && currentSession.CustID > 0 &&
                !string.IsNullOrEmpty(currentSession.VendorID) && !string.IsNullOrEmpty(currentSession.VendorIdentityID)
                && currentSession.IsIT && currentSession.ApplicationMode == AppMode.Parasitic;

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
            SessionManager.CurrentSession = _accountService.CreateSession(user, AppSettings.LicenseeID);
        }

        private IList<LicenseeSiteName> GetLiceseeSiteNameList(string filePath)
        {
            const string LicenseeSiteNameCacheKey = "OneAuthenticationService_LicenseeSiteNameCacheKey";
            const int CacheTimeoutMinute = 30;
            var cacheService = GetMemCacheService();

            var result = cacheService.Get<IList<LicenseeSiteName>>(LicenseeSiteNameCacheKey);

            if (result == null || !result.Any())
            {
                var temp = GetStringFromFile(filePath);
                if (!string.IsNullOrEmpty(temp))
                {
                    result = JsonConvert.DeserializeObject<IList<LicenseeSiteName>>(temp);
                    if (result.Any())
                    {
                        cacheService.Set(LicenseeSiteNameCacheKey, result, DateTime.Now.AddMinutes(CacheTimeoutMinute));
                    }
                }
            }

            return result;
        }
    }
}