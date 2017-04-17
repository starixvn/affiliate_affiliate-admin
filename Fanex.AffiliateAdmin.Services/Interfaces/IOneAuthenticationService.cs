namespace Fanex.AffiliateAdmin.Services.Interfaces
{
    using System.Web;
    using DTO.OneAuth;
    using Fanex.AffiliateAdmin.DTO.Common;

    public interface IOneAuthenticationService
    {
        ResultInfo SignIn(HttpContextBase currentContext, OneAuthToken tokenInfo);

        OneAuthToken GetTokenInfo(string token);
    }
}