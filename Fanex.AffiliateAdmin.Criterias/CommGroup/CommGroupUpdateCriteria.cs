namespace Fanex.AffiliateAdmin.Criterias
{
    public class CommGroupUpdateCriteria : CommGroupInsertUpdateCriteria
    {
        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}