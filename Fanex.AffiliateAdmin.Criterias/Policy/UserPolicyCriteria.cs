namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserPolicyCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets or sets Customer ID.
        /// </summary>
        /// <value>Customer ID.</value>
        public int CustID { get; set; }

        /// <summary>
        /// Gets or sets GroupID.
        /// </summary>
        /// <value>Group ID of user.</value>
        public int GroupID { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_AdminPolicySelByCustID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}