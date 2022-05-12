namespace Fanex.AffiliateAdmin.Criterias.Configuration.AffType
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Select criteria class.
    /// </summary>
    public class AffTypeSelectCriteria : BaseCriteria
    {
        public string Name { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffTypeSelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}