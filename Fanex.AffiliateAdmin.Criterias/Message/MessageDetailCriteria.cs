namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageDetailCriteria : CriteriaBase
    {
        public int? MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_HotNewsDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}