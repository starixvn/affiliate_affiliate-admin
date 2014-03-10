// -----------------------------------------------------------------------
// <copyright file="AffiliateUpdStatusCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateUpdStatusCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int UserId { get; set; }

        public string Status { get; set; }

        public string Password { get; set; }

        public int GroupId { get; set; }

        public bool Closed { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string Type { get; set; }

        public override string GetSettingKey()
        {
            if (!string.IsNullOrEmpty(this.Type))
            {
                return "AffAdmin_AffiliateUpdStatus";
            }

            return "AffAdmin_AffiliateUpdClosedStatus";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
