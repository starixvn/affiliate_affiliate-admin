// -----------------------------------------------------------------------
// <copyright file="NoteViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NoteViewModel
    {
        public int AffId { get; set; }

        public int NoteId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrNoteEmpty")]
        public string Content { get; set; }
    }
}
