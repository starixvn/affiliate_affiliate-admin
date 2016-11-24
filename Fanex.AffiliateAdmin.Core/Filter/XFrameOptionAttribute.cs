namespace Fanex.AffiliateAdmin.Core.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Configurations;
    using Enumerations;
    using Session;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class XFrameOptionAttribute : ActionFilterAttribute
    {
        private const string XFrameOptionAppSettingKey = "XFrameOption";
        private const string XFrameOptionHeaderName = "X-Frame-Options";

        public XFrameOptionAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AddXFrameOption(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private void AddXFrameOption(ActionExecutingContext actionContext)
        {
            var session = SessionManager.CurrentSession;
            var value = session != null && session.ApplicationMode == AppMode.Parasitic ? AppSettings.GetValue(XFrameOptionAppSettingKey)
                : "SAMEORIGIN";

            IEnumerable<string> headerValues = actionContext.HttpContext.Response.Headers.GetValues(XFrameOptionHeaderName);

            if (headerValues != null && headerValues.Any())
            {
                actionContext.HttpContext.Response.Headers.Remove(XFrameOptionHeaderName);
            }

            actionContext.HttpContext.Response.Headers.Add(XFrameOptionHeaderName, value);
        }
    }
}