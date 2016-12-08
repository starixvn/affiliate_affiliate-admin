namespace Fanex.AffiliateAdmin.Services.Interfaces
{
    using Fanex.AffiliateAdmin.DTO.Common;

    public interface IOneBookSecurityWrapper
    {
        OneAuthenticationToken Authenticate(string token);
    }
}