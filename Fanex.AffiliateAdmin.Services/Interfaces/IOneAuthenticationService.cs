namespace Fanex.AffiliateAdmin.Services.Interfaces
{
    using System.Web;
    using Fanex.AffiliateAdmin.DTO.Common;

    public interface IOneAuthenticationService
    {
        ResultInfo SignIn(HttpContextBase currentContext, OneAuthenticationToken tokenInfo);

        OneAuthenticationToken GetTokenInfo(string token);
    }
}