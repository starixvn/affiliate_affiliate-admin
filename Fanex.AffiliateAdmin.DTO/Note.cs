// -----------------------------------------------------------------------
// <copyright file="Note.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Note
    {
        public int NoteId { get; set; }

        public string Content { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedDateStr
        {
            get
            {
                return UpdatedDate.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public int AffId { get; set; }

        public int UpdatedBy { get; set; }

        public string UpdateName { get; set; }
    }
}
