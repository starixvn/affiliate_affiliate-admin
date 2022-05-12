namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Core;
    using Core.Filter;
    using Core.Session;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Constants;
    using Fanex.AffiliateAdmin.Criterias;
    using Models;
    using ONEbook.UM.AspNetMVC.Filters;
    using Resources;

    /// <summary>
    /// Base Controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [Authorization]
    [CheckUMFilter]
    [XFrameOption]
    public class BaseController : Controller
    {
        private readonly IAppSetting _appSetting;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseController" /> class.
        /// </summary>
        public BaseController(IAppSetting appSetting)
        {
            ////Set default language
            var langManager = new LanguageManager();
            langManager.SetCulture(AppSession.Language);

            _appSetting = appSetting;
        }

        protected IAppSetting AppSetting => _appSetting;

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
        protected string GetModelErrorMessage()
        {
            var modelState = ViewData.ModelState.Values.FirstOrDefault();
            if (modelState == null || !modelState.Errors.Any())
            {
                return "Undefined Error";
            }

            return modelState.Errors.First().ErrorMessage;
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
                ErrorMessage = errCode == ErrorCodes.Success ?
                    (string.IsNullOrEmpty(messageSuccess) ? Lang.UpdateSucc : messageSuccess) :
                    (string.IsNullOrEmpty(errorMessage) ? ErrorMessage(errCode) : errorMessage),
            };
        }

        protected JsonResult InvalidModelResult()
        {
            var result = new ResultViewModel
            {
                ErrorCode = ErrorCodes.GeneralErrorCode,
                ErrorMessage = GetModelErrorMessage()
            };

            return Json(result);
        }

        protected JsonResult ModelResult(int errCode)
        {
            var result = GetResultViewModel(errCode, "Ok");

            return Json(result);
        }

        protected void AssignBaseCriteria(BaseCriteria criteria)
        {
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? _appSetting.PageSize : criteria.PageSize;
        }
    }
}