namespace Fanex.AffiliateAdmin.Core.Filter
{
    using Fanex.AffiliateAdmin.Core.Session;
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Authorization Attribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return SessionManager.CurrentSession != null && base.AuthorizeCore(httpContext);
        }

        /// <summary>
        /// Handle Unauthorized Request.
        /// </summary>
        /// <param name="context">Authorization Context.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "NotAuthorized",
                        LogOnUrl = "Account/Logout"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(context);
            }
        }
    }
}