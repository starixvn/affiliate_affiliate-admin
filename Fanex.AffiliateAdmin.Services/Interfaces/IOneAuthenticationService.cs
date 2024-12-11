namespace Fanex.AffiliateAdmin.Services.Interfaces
{
    using DTO.OneAuth;
    using Fanex.AffiliateAdmin.DTO.Common;
    using System.Web;

    public interface IOneAuthenticationService
    {
        ResultInfo SignIn(HttpContextBase currentContext, OneAuthToken tokenInfo);

        OneAuthToken GetTokenInfo(string token);
    }
}