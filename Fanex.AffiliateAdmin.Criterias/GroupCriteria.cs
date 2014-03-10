// -----------------------------------------------------------------------
// <copyright file="GroupCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int? GroupId { get; set; }

        public string GroupName { get; set; }

        [SpParam(SqlParamName = "groupdesc")]
        public string Description { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            string settingKey = string.Empty;
            switch (this.Type)
            {
                case "Add":
                    settingKey = "AffAdmin_Con_GroupInfoIns";
                    break;
                case "Edit":
                    settingKey = "AffAdmin_Con_GroupInfoUpd";
                    break;
                case "Delete":
                    settingKey = "AffAdmin_Con_GroupInfoDel";
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
