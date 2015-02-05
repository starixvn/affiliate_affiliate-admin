namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Account Update Criteria.
    /// </summary>
    public class AccountUpdCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int UserId { get; set; }

        public int GroupId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_AccountUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}