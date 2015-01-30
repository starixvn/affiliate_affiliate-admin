namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Account Update Closed Criteria.
    /// </summary>
    public class AccountUpdClosedCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool Closed { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_AccountUpdClosed";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}