// -----------------------------------------------------------------------
// <copyright file="NoteListCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NoteListCriteria : CriteriaBase
    {
        public int AffId { get; set; }
       
        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_NoteSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
