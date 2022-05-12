namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Text;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Filter;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Services;
    using Models.Message;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.HotNews)]
    public class MessageController : BaseController
    {
        private readonly ISystemSettings _systemSettings;
        private readonly IMessageService _messageService;

        public MessageController(
            IAppSetting appSetting,
            ISystemSettings systemSettings,
            IMessageService messageService) : base(appSetting)
        {
            _systemSettings = systemSettings;
            _messageService = messageService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new MessageViewModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today
            };

            return View(model);
        }

        /// <summary>
        /// Get Message via ajax.
        /// </summary>
        /// <param name="model">Message View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult DataList(MessageViewModel model)
        {
            var criteria = new MessageCriteria
            {
                // Important is an option value, if it is existed in query string and its value is 1, the Important value is true, otherwise false
                // Note: if its value is 0, User name option is selected.
                Important = model.SearchBy == null ? null : (bool?)(model.SearchBy.Value == 1),
                FromDate = model.DateChecked ? (DateTime?)model.FromDate : null,
                ToDate = model.DateChecked ? (DateTime?)model.ToDate : null
            };
            AssignBaseCriteria(criteria);

            var data = _messageService.GetMessages(criteria);
            return Json(new
            {
                List = data,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        [HttpGet]
        public ActionResult AddMessage()
        {
            var model = new MessageAddViewModel();
            return View(model);
        }

        /// <summary>
        /// Add Message.
        /// </summary>
        /// <param name="values">Form Collection.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult AddMessage(FormCollection values)
        {
            if (ModelState.IsValid)
            {
                bool isValid;
                var messageXml = GenMessageXml(values, out isValid);
                if (!isValid)
                {
                    ModelState.AddModelError("Message", Lang.AddEmptyMessError);
                }
                else
                {
                    var session = SessionManager.CurrentSession;

                    var criteria = new MessageAddCriteria
                    {
                        Important = values["Important"].ToLower().Contains("true"),
                        Xml = messageXml,
                        UserID = session.CustID,
                        AdminUserName = session.Username,
                        IP = Remote.GetClientIPAddress()
                    };

                    _messageService.AddMessage(criteria);
                    ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
                }
            }

            return AddMessage();
        }

        [HttpGet]
        public ActionResult EditMessage(int? msgId)
        {
            var criteria = new MessageDetailCriteria
            {
                MsgId = msgId
            };

            var model = _messageService.MessageDetail(criteria);
            return View(model);
        }

        /// <summary>
        /// Edit Message.
        /// </summary>
        /// <param name="values">Form Collection.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult EditMessage(FormCollection values)
        {
            int? msgId = null;
            if (ModelState.IsValid)
            {
                msgId = Convert.ToInt32(values["MsgId"]);
                var messageXml = GenMessageXml(values, out bool isValid);
                if (!isValid)
                {
                    ModelState.AddModelError("Message", Lang.AddEmptyMessError);
                }
                else
                {
                    var session = SessionManager.CurrentSession;

                    var criteria = new MessageUpdateCriteria
                    {
                        MsgId = Convert.ToInt32(values["MsgId"]),
                        Important = values["Important"].ToLower().Contains("true"),
                        Xml = messageXml,
                        UserID = session.CustID,
                        AdminUserName = session.Username,
                        IP = Remote.GetClientIPAddress(),
                    };

                    _messageService.UpdateMessage(criteria);
                    ViewBag.Result = GetResultViewModel(criteria.ErrCode);
                }
            }

            return EditMessage(msgId);
        }

        /// <summary>
        /// Delete Message.
        /// </summary>
        /// <param name="model">Message Delete View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult DeleteMessage(MessageDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new MessageDeleteCriteria
                {
                    MsgIds = model.MsgIds,
                    UserID = SessionManager.CurrentSession.CustID,
                    IP = Remote.GetClientIPAddress()
                };

                _messageService.DeleteMessage(criteria);
                return ModelResult(criteria.ErrCode);
            }

            return InvalidModelResult();
        }

        /// <summary>
        /// Generate Message object in xml format from FormCollection.
        /// This xml will be passed to stored procedure "AFFADMIN_HOTNEWSUPD".
        /// </summary>
        /// <param name="values">Form Collection.</param>
        /// <param name="isValid">Output is Valid.</param>
        /// <returns>Message object in xml format.</returns>
        private string GenMessageXml(FormCollection values, out bool isValid)
        {
            isValid = false;
            var stringBuilder = new StringBuilder("<root>");
            foreach (var key in values.AllKeys)
            {
                foreach (var language in _systemSettings.AffLanguages)
                {
                    if (key == string.Format("LangMessages[{0}]", language.LanguageID))
                    {
                        var message = values[key].Trim();
                        if (!string.IsNullOrEmpty(message))
                        {
                            isValid = true;
                        }

                        stringBuilder.AppendFormat("<item><code>{0}</code><value>{1}</value></item>", language.LanguageID, message);
                        break;
                    }
                }
            }

            stringBuilder.Append("</root>");

            return stringBuilder.ToString();
        }
    }
}