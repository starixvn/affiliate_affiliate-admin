// -----------------------------------------------------------------------
// <copyright file="NoteService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services.Interface;
    using Fanex.Common.Helper;
    
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
        public Note GetNote(NoteCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return GetData<Note>(criteria).FirstOrDefault();
        }

        /// <summary>
        /// Insert note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        public void InsertNote(NoteCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        public void UpdateNote(NoteCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Delete note.
        /// </summary>
        /// <param name="criteria">Note criteria.</param>
        public void DeleteNote(NoteCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}
