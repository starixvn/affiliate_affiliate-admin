namespace Fanex.AffiliateAdmin.Services.Impl.Core
{
    using System.Configuration;
    using DTO.Common;
    using Interface.Core;
    using ONEbook.Security;

    public class OneBookSecurityWrapper : IOneBookSecurityWrapper
    {
        public OneAuthenticationToken Authenticate(string token)
        {
            OneAuthenticationToken result = null;

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString_ClearinghouseDB"].ConnectionString;
            Auth<OneAuthenticationToken> oneBookSecurity = new Auth<OneAuthenticationToken>(connectionString);
            oneBookSecurity.Authenticate(token, ref result);

            return result;
        }
    }
}