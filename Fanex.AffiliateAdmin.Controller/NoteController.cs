// -----------------------------------------------------------------------
// <copyright file="NoteController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate)]
    public class NoteController : BaseController
    {
        #region Services

        private readonly INoteService _noteService;

        #endregion Services

        #region Constructor

        public NoteController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteController" /> class.
        /// </summary>
        /// <param name="noteService">Note Service.</param>
        public NoteController(INoteService noteService)
        {
            _noteService = noteService ?? new NoteService();
        }

        #endregion Constructor

        /// <summary>
        /// Show view Note.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Status = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).Status;

            return View();
        }

        /// <summary>
        /// Load list notes.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult ListNote()
        {
            var affId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId;
            return Json(new
            {
                List = this._noteService.GetNoteList(new NoteListCriteria { AffId = affId })
            });
        }

        /// <summary>
        /// Add affiliate note.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult AddNote()
        {
            return View(new NoteViewModel());
        }

        /// <summary>
        /// Add affiliate note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult AddNote(NoteInsCriteria criteria)
        {
            if (ModelState.IsValid && criteria.IsValid())
            {
                criteria.AffId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId;
                criteria.Ip = Remote.GetClientIPAddress();

                var session = SessionManager.CurrentSession;
                criteria.AdminUserName = session.Username;
                criteria.UpdatedBy = session.CustID;

                this._noteService.InsertNote(criteria);
                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.SuccNoteAdd : ErrorMessage(criteria.ErrCode)
                };
            }

            return AddNote();
        }

        /// <summary>
        /// Edit affiliate note.
        /// </summary>
        /// <param name="noteId">Note Input Id.</param>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult EditNote(int noteId)
        {
            var note = this._noteService.GetNote(new NoteSelCriteria
            {
                AffId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId,
                NoteId = noteId
            });

            return View(new NoteViewModel { Content = note.Content, NoteId = noteId });
        }

        /// <summary>
        /// Edit affiliate note.
        /// </summary>
        /// <param name="criteria">Note input criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult EditNote(NoteUpdCriteria criteria)
        {
            criteria.AffId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId;
            criteria.Ip = Remote.GetClientIPAddress();

            var session = SessionManager.CurrentSession;
            criteria.UpdatedBy = session.CustID;
            criteria.AdminUserName = session.Username;

            this._noteService.UpdateNote(criteria);

            ViewBag.Result = new ResultViewModel
            {
                ErrorCode = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? Lang.UpdateSucc : ErrorMessage(criteria.ErrCode)
            };

            return View();
        }
    }
}