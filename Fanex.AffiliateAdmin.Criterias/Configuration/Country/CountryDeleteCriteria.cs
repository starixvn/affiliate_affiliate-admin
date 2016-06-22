namespace Fanex.AffiliateAdmin.Criterias.Configuration.Country
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CountryDelete Criteria class.
    /// </summary>
    public class CountryDeleteCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string IDList { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CountryDel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(IDList);
        }
    }
}