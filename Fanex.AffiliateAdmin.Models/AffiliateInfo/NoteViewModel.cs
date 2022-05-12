// -----------------------------------------------------------------------
// <copyright file="NoteViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models.AffiliateInfo
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NoteViewModel
    {
        public int NoteId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrNoteEmpty")]
        public string Content { get; set; }
    }
}
