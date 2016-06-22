namespace Fanex.AffiliateAdmin.Criterias.Configuration.AffType
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Add criteria class.
    /// </summary>
    public class AffTypeAddCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffTypeIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}