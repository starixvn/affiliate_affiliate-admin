// -----------------------------------------------------------------------
// <copyright file="NoteCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NoteCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int? NoteId { get; set; }

        public string Content { get; set; }

        public int UpdatedBy { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string Type { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPage { get; set; }

        public int RowCount { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            string settingKey = string.Empty;

            switch (Type)
            {
                case "List":
                    settingKey = "AffAdmin_NoteSel";
                    break;
                case "Add":
                    settingKey = "AffAdmin_NoteIns";
                    break;
                case "Update":
                    settingKey = "AffAdmin_NoteUpd";
                    break;
                case "Delete":
                    settingKey = "AffAdmin_NoteDel";
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
