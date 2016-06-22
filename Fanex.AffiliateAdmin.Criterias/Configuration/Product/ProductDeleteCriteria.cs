namespace Fanex.AffiliateAdmin.Criterias.Configuration.Product
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ProductDelete Criteria class.
    /// </summary>
    public class ProductDeleteCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string IDList { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ProductDel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(IDList);
        }
    }
}