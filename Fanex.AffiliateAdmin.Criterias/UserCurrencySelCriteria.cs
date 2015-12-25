namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// User Currency Select Criteria.
    /// </summary>
    public class UserCurrencySelCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_UserCurrencySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}