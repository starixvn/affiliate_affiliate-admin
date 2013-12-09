// --------------------------------------------------------------------------
// <copyright file="AuthorizationAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.05">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Core.Filter
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Logging;

    /// <summary>
    /// Authorization Attribute class.
    /// </summary>
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// On Authorization.
        /// </summary>
        /// <param name="filterContext">Authorization Context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            try
            {
                if (SessionManager.CurrentSession == null)
                {
                    DoUnAuthorized(filterContext);
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Do Un-Authorized.
        /// </summary>
        /// <param name="filterContext">Authorization Context.</param>
        private void DoUnAuthorized(AuthorizationContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.StatusDescription = string.Empty;
                response.Write(Lang.UnauthorizedMsg);
                response.End();
            }
            else
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                response.Redirect(FormsAuthentication.LoginUrl);
            }
        }
    }
}