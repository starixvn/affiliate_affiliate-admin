namespace Fanex.AffiliateAdmin.Services.Impl.Core
{
    using System.Configuration;
    using DTO.OneAuth;
    using Interfaces;
    using ONEbook.Security;

    public class OneBookSecurityWrapper : IOneBookSecurityWrapper
    {
        public OneAuthToken Authenticate(string token)
        {
            OneAuthToken result = null;

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString_ClearinghouseDB"].ConnectionString;
            Auth<OneAuthToken> oneBookSecurity = new Auth<OneAuthToken>(connectionString);
            oneBookSecurity.Authenticate(token, ref result);

            return result;
        }
    }
}