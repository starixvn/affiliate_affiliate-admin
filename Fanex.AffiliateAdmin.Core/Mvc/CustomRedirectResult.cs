using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Fanex.AffiliateAdmin.Core.Mvc
{
    public class CustomRedirectResult : ActionResult
    {
        private string Url { get; set; }
        public CustomRedirectResult(string url)
        {
            this.Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Redirect(this.Url, true);
        }
    }
}
