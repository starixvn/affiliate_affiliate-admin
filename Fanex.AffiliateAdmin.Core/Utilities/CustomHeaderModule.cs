namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Web;

    /// <summary>
    /// Custom Header Module.
    /// </summary>
    public class CustomHeaderModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostReleaseRequestState += ApplicationPostReleaseRequestState;
        }

        public void Dispose()
        {
        }

        private void ApplicationPostReleaseRequestState(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("ETag");
        }
    }
}
