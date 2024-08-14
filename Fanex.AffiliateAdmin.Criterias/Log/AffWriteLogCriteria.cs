namespace Fanex.AffiliateAdmin.Criterias
{
    public class AffWriteLogCriteria : WriteLogBaseCriteria
    {
        public string TypeName { get; set; }

        public override string GetSettingKey() => "Aff_WriteLog";
    }
}
