namespace Fanex.AffiliateAdmin.Services.Interface.Core
{
    using Fanex.AffiliateAdmin.DTO.Common;

    public interface IOneBookSecurityWrapper
    {
        OneAuthenticationToken Authenticate(string token);
    }
}