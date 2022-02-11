namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageDeleteCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "listmsgid")]
        public string MsgIds { get; set; }

        public int ErrCode { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_HotNewsDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}