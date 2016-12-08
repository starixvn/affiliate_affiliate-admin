// -----------------------------------------------------------------------
// <copyright file="INoteService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Get note list.
        /// </summary>
        /// <param name="criteria">Note list criteria.</param>
        /// <returns>List note.</returns>
        IEnumerable<Note> GetNoteList(NoteListCriteria criteria);

        /// <summary>
        /// Get note information.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        /// <returns>Note information.</returns>
        Note GetNote(NoteSelCriteria criteria);

        /// <summary>
        /// Insert note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        void InsertNote(NoteInsCriteria criteria);

        /// <summary>
        /// Update note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        void UpdateNote(NoteUpdCriteria criteria);

        /// <summary>
        /// Delete note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        void DeleteNote(NoteDelCriteria criteria);
    }
}
