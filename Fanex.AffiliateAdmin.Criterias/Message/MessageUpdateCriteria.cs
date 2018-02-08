namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageUpdateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ID")]
        public int MsgId { get; set; }

        public bool Important { get; set; }

        public string Xml { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string AdminUserName { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_HotNewsUpd";
        }

        public override bool IsValid()
        {
            if (Xml == null)
            {
                return false;
            }

            return true;
        }
    }
}