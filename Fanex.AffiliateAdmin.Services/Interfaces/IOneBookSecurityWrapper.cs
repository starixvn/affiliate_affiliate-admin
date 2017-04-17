namespace Fanex.AffiliateAdmin.Services.Interfaces
{
    using DTO.OneAuth;

    public interface IOneBookSecurityWrapper
    {
        OneAuthToken Authenticate(string token);
    }
}