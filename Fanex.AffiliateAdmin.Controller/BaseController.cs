// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.07.12">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using Core.Filter;
    using Core.Session;
    using Models;
    using Resources;

    /// <summary>
    /// Base Controller.
    /// </summary>
    [Undermaintenance]
    [Authorization]
    public class BaseController : Controller
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseController" /> class.
        /// </summary>
        public BaseController()
        {
            ////Set default language
            var langManager = new LanguageManager();
            langManager.SetCulture(AppSession.Language);

            /*
            // Set default currency
            //if (CurrentSession != null)
            //{
            //    FormaterCurrency.SetDefaultCurrency(CurrentSession.Currency);
            //}
             */
        }

        protected ICurrentSession CurrentSession
        {
            get { return SessionManager.CurrentSession; }
        }

        protected ActionResult ErrorPage(string message)
        {
            return View("~/Views/Notify/Message.cshtml", (object)message);
        }

        protected string ErrorMessage(int errCode)
        {
            return ErrorMsg.ResourceManager.GetString("Msg" + errCode);
        }

        protected ResultViewModel GetResultViewModel(int errCode, string messageSuccess = "", string errorMessage = "")
        {            
            return new ResultViewModel
            {
                ErrorCode = errCode,
                Message = errCode == 0 ?
                    (string.IsNullOrEmpty(messageSuccess) ? Lang.UpdateSucc : messageSuccess) : 
                    (string.IsNullOrEmpty(errorMessage) ? ErrorMessage(errCode) : errorMessage),
                Success = errCode == 0
            };
        }

        /// <summary>
        /// Get Error message.
        /// </summary>
        /// <returns>Error message string.</returns>
        protected string GetModelErrorMessage()
        {
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    return error.ErrorMessage;
                }
            }

            return "Undefined Error";
        }

        protected string[] GetModelErrorMessages()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToArray();
        }

        protected JsonResult InvalidModelResult()
        {
            var result = new ResultViewModel
            {
                ErrorCode = -1,
                Success = false,
                Message = GetModelErrorMessage()
            };

            return Json(result);
        }

        protected JsonResult ModelResult(int errCode)
        {
            var result = new ResultViewModel
            {
                ErrorCode = errCode,
                Success = errCode == 0,
                Message = errCode == 0 ? "OK" : ErrorMessage(errCode)
            };

            return Json(result);
        }
    }
}