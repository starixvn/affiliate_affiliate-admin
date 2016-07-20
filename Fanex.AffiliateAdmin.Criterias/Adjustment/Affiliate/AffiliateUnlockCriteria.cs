namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Affiliate Unlock Criteria.
    /// </summary>
    public class AffiliateUnlockCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int AffId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateUnlock";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
