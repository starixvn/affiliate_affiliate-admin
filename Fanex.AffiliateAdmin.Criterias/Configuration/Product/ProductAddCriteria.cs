namespace Fanex.AffiliateAdmin.Criterias.Configuration.Product
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ProductAdd Criteria class.
    /// </summary>
    public class ProductAddCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string BettypeList { get; set; }

        public int? API_ProductID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ProductIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ProductName);
        }
    }
}