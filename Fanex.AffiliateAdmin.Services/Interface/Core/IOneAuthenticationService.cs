namespace Fanex.AffiliateAdmin.Services.Interface.Core
{
    using System.Web;
    using Fanex.AffiliateAdmin.DTO.Common;

    public interface IOneAuthenticationService
    {
        LicenseeSiteName SingleLicenseeSiteName(string siteName);

        ResultInfo SignIn(HttpContextBase currentContext, int licenseeId, OneAuthenticationToken tokenInfo);

        OneAuthenticationToken GetTokenInfo(string token);
    }
}