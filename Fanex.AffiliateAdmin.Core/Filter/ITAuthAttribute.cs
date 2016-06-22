namespace Fanex.AffiliateAdmin.Core.Filter
{
    using System;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Session;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ITAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionManager.CurrentSession != null && !SessionManager.CurrentSession.IsIT)
            {
                filterContext.HttpContext.Response.Redirect("~/notify/PageNotFound");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}