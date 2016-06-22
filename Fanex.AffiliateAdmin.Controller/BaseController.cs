namespace Fanex.AffiliateAdmin.Controller
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Core;
    using Core.Filter;
    using Core.Session;
    using Models;
    using ONEbook.UM.Filters;
    using Resources;

    /// <summary>
    /// Base Controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [Authorization]
    [CheckUMFilter]
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
        }

        protected string ErrorMessage(int errCode)
        {
            return ErrorMsg.ResourceManager.GetString("Msg" + errCode);
        }

        protected ActionResult ErrorPage(string message)
        {
            return View("~/Views/Notify/Message.cshtml", (object)message);
        }

        protected ActionResult RedirectToAccessDenied()
        {
            return new RedirectResult("~/Notify/AccessDenied");
        }

        /// <summary>
        /// Get Error message.
        /// </summary>
        /// <returns>Error message string.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Accept this method")]
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