namespace Fanex.AffiliateAdmin.Core.Filter
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Configurations;
    using Enumerations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Authorization Attribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class FunctionAuthAttribute : ActionFilterAttribute
    {
        private const int AccessDeniedErrorCode = 500;

        public string Function { get; set; }

        public bool IsHostPolicy { get; set; }

        public bool CurrenciesRequired { get; set; }

        /// <summary>
        /// Action filter for under maintenance service.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var hasPermission = false;

            var currentSession = SessionManager.CurrentSession;
            if (currentSession != null)
            {
                hasPermission = IsHostPolicy ?
                    currentSession.HasHostPolicy(Function) :
                    currentSession.HasReadPolicy(Function);

                if (hasPermission && CurrenciesRequired && !currentSession.IsIT)
                {
                    hasPermission = currentSession.CurrencyList.Any();
                }

                if (!currentSession.IsIT && Function == FunctionPolicyNames.UserControl)
                {
                    hasPermission = false;
                }
            }

            if (!hasPermission)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var data = new { ErrorCode = AccessDeniedErrorCode, Message = ErrorMsg.Msg500 };
                    filterContext.Result = new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/notify/AccessDenied");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}