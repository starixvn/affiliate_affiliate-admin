namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    public interface IFormsAuthenticationWrapper
    {
        string LoginUrl { get; }

        void SignOut();

        void SetAuthCookie(string userName, bool createPersistentCookie);
    }
}
