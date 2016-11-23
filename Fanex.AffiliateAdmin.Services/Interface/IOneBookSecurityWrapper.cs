namespace Fanex.AffiliateAdmin.Services.Interface
{
    using Fanex.AffiliateAdmin.DTO.Common;

    public interface IOneBookSecurityWrapper
    {
        OneAuthenticationToken Authenticate(string token);
    }
}