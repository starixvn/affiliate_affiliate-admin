// --------------------------------------------------------------------------
// <copyright file="MemberController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.8">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Filter;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[Affiliate]")]
    public class MemberController : BaseController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}