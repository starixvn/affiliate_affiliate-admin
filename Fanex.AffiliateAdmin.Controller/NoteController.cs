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
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[Affiliate]")]
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
            ViewBag.Status = this.CurrentSession.Affiliate.Status;

            return View();
        }

        /// <summary>
        /// Load list notes.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult ListNote()
        {
            return Json(new
            {
                List = this._noteService.GetNoteList(new NoteListCriteria { AffId = this.CurrentSession.Affiliate.AffiliateId, LicenseeId = AppSettings.LicenseeID })
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
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;
                criteria.UpdatedBy = this.CurrentSession.CustID;
                criteria.LicenseeId = AppSettings.LicenseeID;

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
                AffId = this.CurrentSession.Affiliate.AffiliateId,
                NoteId = noteId,
                LicenseeId = AppSettings.LicenseeID
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
            criteria.Ip = Remote.GetClientIPAddress();
            criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;
            criteria.UpdatedBy = this.CurrentSession.CustID;
            criteria.LicenseeId = AppSettings.LicenseeID;

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