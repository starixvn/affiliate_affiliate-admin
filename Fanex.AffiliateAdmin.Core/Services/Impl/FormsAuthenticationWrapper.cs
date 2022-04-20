namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using System.Web.Security;

    public class FormsAuthenticationWrapper : IFormsAuthenticationWrapper
    {
        public string LoginUrl => FormsAuthentication.LoginUrl;

        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
