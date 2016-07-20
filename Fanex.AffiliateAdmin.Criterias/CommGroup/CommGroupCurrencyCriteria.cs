namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommGroupCurrencyCriteria : CriteriaBase
    {
         public int UserId { get; set; }

         public string Currency { get; set; }

         public int LicenseeId { get; set; }

         public override string GetSettingKey()
         {
             return "AffAdmin_CommGroupSelListNormal";
         }

         public override bool IsValid()
         {
             return true;
         }
    }
}
