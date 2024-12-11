// -----------------------------------------------------------------------
// <copyright file="NoteService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.Common.Helper;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NoteService : QueryService, INoteService
    {
        /// <summary>
        /// Get note list.
        /// </summary>
        /// <param name="criteria">Note list criteria.</param>
        /// <returns>List note.</returns>
        public IEnumerable<Note> GetNoteList(NoteListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<Note>(criteria);
        }

        /// <summary>
        /// Get note information.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        /// <returns>Note information.</returns>
        public Note GetNote(NoteSelCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<Note>(criteria).FirstOrDefault();
        }

        /// <summary>
        /// Insert note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        public void InsertNote(NoteInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        public void UpdateNote(NoteUpdCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Delete note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        public void DeleteNote(NoteDelCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}
