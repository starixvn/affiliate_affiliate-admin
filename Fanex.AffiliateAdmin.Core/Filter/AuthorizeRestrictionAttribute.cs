namespace Fanex.AffiliateAdmin.Core.Filter
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Enumerations;
    using Session;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AuthorizeRestrictionAttribute : AuthorizeAttribute
    {
        private readonly AppMode _appMode;

        public AuthorizeRestrictionAttribute(AppMode mode)
        {
            _appMode = mode;
        }

        #region Override

        /// <summary>
        /// On authorization.
        /// </summary>
        /// <param name="filterContext"> Parameter value.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);
                var currentSession = SessionManager.CurrentSession;
                var authorized = currentSession != null && currentSession.ApplicationMode == _appMode;
                if (!authorized)
                {
                    var routeValue = new
                    {
                        controller = "Notify",
                        action = "OneTokenRestriction",
                        code = "700"
                    };

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValue));
                }
            }
        }

        #endregion Override
    }
}
